using Catalogo.Domain.Entities;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Context;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Infrastructure.Repositories
{
    public class CategoriaRepository : RepositoryBase, ICategoriaRepository
    {
        public CategoriaRepository(IUnitOfWorkRepository uoW) : base(uoW)
        {
        }

        public async Task<int> CreateAsync(Categoria category)
        {
            return
                await _connection.ExecuteAsync("INSERT INTO catalagodapper.categoria (Nome, ImagemUrl) VALUES (@Nome, @ImagemUrl);", 
                    category,
                    transaction: _transaction
                );
        }

        public async Task<Categoria> GetByIdAsync(int? id)
        {
            return await _connection.QueryFirstOrDefaultAsync<Categoria>(
                "SELECT Id, Nome, ImagemUrl FROM catalagodapper.categoria WHERE Id=@Id;",
                new { id },
                _transaction
            );
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            return await _connection.QueryAsync<Categoria>(
                "SELECT Id, Nome, ImagemUrl FROM catalagodapper.categoria;",
                _transaction
            );
        }

        public async Task<int> RemoveAsync(Categoria category)
        {
            return
                await _connection.ExecuteAsync(
                    "DELETE FROM catalagodapper.categoria WHERE Id=@Id;",
                    new { category.Id },
                    _transaction
                );
        }

        public async Task<int> UpdateAsync(Categoria category)
        {
            return 
                await _connection.ExecuteAsync(
                    "UPDATE catalagodapper.categoria SET Nome=@Nome, ImagemUrl=@ImagemUrl WHERE Id=@Id;",
                    category,
                    _transaction
                );
        }
    }
}
