using AutoMapper;
using Catalogo.Application.DTOs;
using Catalogo.Application.Exceptions;
using Catalogo.Application.Interfaces;
using Catalogo.Application.UoW;
using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IUnitOfWork _uow;
        private ICategoriaRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriaService(
            IUnitOfWork uow,
            ICategoriaRepository categoryRepository,
            IMapper mapper)
        {
            _uow = uow;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetCategorias()
        {
            IEnumerable<Categoria> categoriesEntity;

            using (await _uow.OpenConnectionAsync())
                categoriesEntity = await _categoryRepository.GetCategoriasAsync();

            return _mapper.Map<IEnumerable<CategoriaDTO>>(categoriesEntity);
        }

        public async Task<CategoriaDTO> GetById(int? id)
        {
            Categoria categoryEntity;

            using (await _uow.OpenConnectionAsync())
                categoryEntity = await _categoryRepository.GetByIdAsync(id);

            return _mapper.Map<CategoriaDTO>(categoryEntity);
        }

        public async Task Add(CategoriaDTO categoryDto)
        {
            var categoryEntity = _mapper.Map<Categoria>(categoryDto);

            categoryEntity.Validate();

            using (await _uow.BeginTransactionAsync())
            {
                if ((await _categoryRepository.GetByName(categoryEntity.Nome)) is not null)
                    throw new ConflictException($"Category with name {categoryEntity.Nome} already exists.");

                await _categoryRepository.CreateAsync(categoryEntity);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task Update(CategoriaDTO categoryDto)
        {
            var categoryEntity = _mapper.Map<Categoria>(categoryDto);

            categoryEntity.Validate();

            using (await _uow.BeginTransactionAsync())
            {
                var currentlyCategoryBd = (await _categoryRepository.GetByIdAsync(categoryEntity.Id))
                    ?? throw new NotFoundException($"Category with id {categoryEntity.Id} not found.");

                if (!currentlyCategoryBd.Nome.Equals(categoryEntity.Nome) &&
                    (await _categoryRepository.GetByName(categoryEntity.Nome)) is not null)
                    throw new ConflictException($"Category with name {categoryEntity.Nome} already exists.");

                await _categoryRepository.UpdateAsync(categoryEntity);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task Remove(int? id)
        {
            using (await _uow.BeginTransactionAsync())
            {
                var categoryEntity = _categoryRepository.GetByIdAsync(id).Result;
                await _categoryRepository.RemoveAsync(categoryEntity);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
