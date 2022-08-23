using Dapper;
using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Infrastructure.Repositories
{
    public class ProdutoRepository : RepositoryBase, IProdutoRepository
    {
        public ProdutoRepository(IUnitOfWorkRepository uoW) : base(uoW)
        {
        }

        public async Task<int> CreateAsync(Produto product)
        {
            return
                await _connection.ExecuteAsync(
                    "INSERT INTO catalagodapper.produto (Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, IdCategoria) VALUES (@Nome, @Descricao, @Preco, @ImagemUrl, @Estoque, @DataCadastro, @IdCategoria);",
                    product,
                    _transaction
                );
        }

        public async Task<Produto> GetByIdAsync(int? id)
        {
            return await _connection.QueryFirstOrDefaultAsync<Produto>(
                "SELECT Id, Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, IdCategoria FROM catalagodapper.produto WHERE Id=@Id;",
                new { id },
                _transaction
            );
        }

        public async Task<Produto> GetByName(string name)
        {
            return
                await _connection.QueryFirstOrDefaultAsync<Produto>(
                    @"SELECT Id, Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, IdCategoria  FROM catalagodapper.produto
                        WHERE UPPER(NOME)=@Name;",
                    new { name },
                    _transaction
                );
        }

        public async Task<IEnumerable<Produto>> GetProdutosAsync()
        {
            return await _connection.QueryAsync<Produto>(
                "SELECT Id, Nome, Descricao, Preco, ImagemUrl, Estoque, DataCadastro, IdCategoria FROM catalagodapper.produto;",
                _transaction
            );
        }

        public async Task<int> RemoveAsync(Produto product)
        {
            return await _connection.ExecuteAsync(
               "DELETE FROM catalagodapper.produto WHERE Id=@Id;",
               new { product.Id },
               _transaction
            );
        }

        public async Task<int> UpdateAsync(Produto product)
        {
            return
                await _connection.ExecuteAsync(
                    @"UPDATE catalagodapper.produto 
                        SET Nome=@Nome, Descricao=@Descricao, Preco=@Preco, ImagemUrl=@ImagemUrl, Estoque=@Estoque, DataCadastro=@DataCadastro, IdCategoria=@IdCategoria
                        WHERE Id=@Id;",
                    product,
                    _transaction
                );
        }
    }
}
