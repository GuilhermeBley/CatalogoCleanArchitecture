using Catalogo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogo.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetProdutosAsync();
        Task<Produto> GetByIdAsync(int? id);
        Task<Produto> GetByName(string name);
        Task<int> CreateAsync(Produto product);
        Task<int> UpdateAsync(Produto product);
        Task<int> RemoveAsync(Produto product);
    }
}
