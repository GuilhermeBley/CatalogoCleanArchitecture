using System;
using System.Data;
using System.Threading.Tasks;

namespace Catalogo.Infrastructure.Context
{
    /// <summary>
    /// Unit of work give a shared connection to repository
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Connection already open
        /// </summary>
        /// <returns>async result of <see cref="IDbConnection"/> opened</returns>
        Task<IDbConnection> GetConn();

        /// <summary>
        /// Commit
        /// </summary>
        /// <returns>async</returns>
        Task CommitAsync();

        /// <summary>
        /// Roll back
        /// </summary>
        /// <returns>async</returns>
        Task RollBackAsync();
    }
}

