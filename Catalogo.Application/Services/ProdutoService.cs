using AutoMapper;
using Catalogo.Application.DTOs;
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

            using (await _uoW.BeginTransactionAsync())
            {
                await _productRepository.CreateAsync(productEntity);
                await _uoW.SaveChangesAsync();
            }
        }

        public async Task Update(ProdutoDTO productDto)
        {
            var productEntity = _mapper.Map<Produto>(productDto);

            using (await _uoW.BeginTransactionAsync())
            {
                if (await )

                await _productRepository.UpdateAsync(productEntity);
                await _uoW.SaveChangesAsync();
            }
        }

        public async Task Remove(int? id)
        {
            Produto productEntity;
            using (await _uoW.OpenConnectionAsync())
                productEntity = await _productRepository.GetByIdAsync(id);

            using (await _uoW.BeginTransactionAsync())
            {
                
                await _productRepository.RemoveAsync(productEntity);
                await _uoW.SaveChangesAsync();
            }
        }
    }
}
