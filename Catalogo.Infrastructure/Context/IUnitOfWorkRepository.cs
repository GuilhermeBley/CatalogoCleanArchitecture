using Catalogo.Application.UoW;
using System.Data;

namespace Catalogo.Infrastructure.Context
{
    /// <summary>
    /// Give a shared connection to repositorys
    /// </summary>
    public interface IUnitOfWorkRepository : IUnitOfWork
    {
        /// <summary>
        /// Avaliable connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Avaliable Transaction
        /// </summary>
        IDbTransaction Transaction { get; }
    }
}

