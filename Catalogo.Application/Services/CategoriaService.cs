using AutoMapper;
using Catalogo.Application.DTOs;
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
            Categoria categoryEntity;

            using (await _uow.BeginTransactionAsync())
            {
                categoryEntity = _mapper.Map<Categoria>(categoryDto);
                await _categoryRepository.CreateAsync(categoryEntity);
                await _uow.SaveChangesAsync();
            }
        }

        public async Task Update(CategoriaDTO categoryDto)
        {
            using (await _uow.BeginTransactionAsync())
            {
                var categoryEntity = _mapper.Map<Categoria>(categoryDto);
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
