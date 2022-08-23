using AutoMapper;
using Catalogo.Application.DTOs;
using Catalogo.Application.Exceptions;
using Catalogo.Application.Interfaces;
using Catalogo.Application.UoW;
using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IUnitOfWork _uoW;
        private IProdutoRepository _productRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;
        public ProdutoService(
            IUnitOfWork uoW,
            IProdutoRepository productRepository,
            ICategoriaRepository categoriaRepository,
            IMapper mapper)
        {
            _uoW = uoW;
            _productRepository = productRepository;
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProdutoDTO>> GetProdutos()
        {
            IEnumerable<Produto> productsEntity;

            using (await _uoW.OpenConnectionAsync())
                productsEntity = await _productRepository.GetProdutosAsync();

            return _mapper.Map<IEnumerable<ProdutoDTO>>(productsEntity);
        }

        public async Task<ProdutoDTO> GetById(int? id)
        {
            Produto productEntity;

            using (await _uoW.OpenConnectionAsync())
                productEntity = await _productRepository.GetByIdAsync(id);

            return _mapper.Map<ProdutoDTO>(productEntity);
        }

        public async Task Add(ProdutoDTO productDto)
        {
            var productEntity = _mapper.Map<Produto>(productDto);

            productEntity.Validate();

            using (await _uoW.BeginTransactionAsync())
            {
                if ((await _productRepository.GetByName(productDto.Nome)) is not null)
                    throw new ConflictException($"Product with name {productDto.Nome} already exists.");
                if ((await _categoriaRepository.GetByIdAsync(productDto.IdCategoria)) is null)
                    throw new NotFoundException($"Category with id {productDto.IdCategoria} not found.");

                await _productRepository.CreateAsync(productEntity);
                await _uoW.SaveChangesAsync();
            }
        }

        public async Task Update(ProdutoDTO productDto)
        {
            var productEntity = _mapper.Map<Produto>(productDto);

            productEntity.Validate();

            using (await _uoW.BeginTransactionAsync())
            {
                Produto productDb = (await _productRepository.GetByIdAsync(productDto.Id)) 
                    ?? throw new NotFoundException($"Product with id {productDto.Id} not found.");
                
                if (!productDb.Nome.Equals(productEntity.Nome) &&
                    (await _productRepository.GetByName(productEntity.Nome)) is not null)
                    throw new ConflictException($"Product with name {productDto.Nome} already exists.");

                if ((await _categoriaRepository.GetByIdAsync(productDto.IdCategoria)) is null)
                    throw new NotFoundException($"Category with id {productDto.IdCategoria} not found.");

                await _productRepository.UpdateAsync(productEntity);
                await _uoW.SaveChangesAsync();
            }
        }

        public async Task Remove(int? id)
        {
            Produto productEntity;
            using (await _uoW.OpenConnectionAsync())
                productEntity = await _productRepository.GetByIdAsync(id);

            if (productEntity is null)
                throw new NotFoundException($"Product with id {id} not found.");

            using (await _uoW.BeginTransactionAsync())
            {
                await _productRepository.RemoveAsync(productEntity);
                await _uoW.SaveChangesAsync();
            }
        }
    }
}
