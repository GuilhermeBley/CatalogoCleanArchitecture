using Catalogo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetCategoriasAsync();
        Task<Categoria> GetByIdAsync(int? id);
        Task<int> CreateAsync(Categoria categoria);
        Task<int> UpdateAsync(Categoria categoria);
        Task<int> RemoveAsync(Categoria categoria);
    }
}
