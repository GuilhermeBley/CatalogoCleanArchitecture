using System;
using System.Collections.Generic;
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
        /// Verify if has there connection
        /// </summary>
        bool HasConnection { get; }

        /// <summary>
        /// Connection already open
        /// </summary>
        /// <returns>async result of <see cref="IDbConnection"/> opened</returns>
        Task<IDbConnection> CreateConnectionAsync();

        /// <summary>
        /// Commit
        /// </summary>
        /// <returns>async</returns>
        Task CommitAsync();
        
        /// <summary>
        /// Execute command
        /// </summary>
        /// <returns>int - rows affected</returns>
        Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Roll back
        /// </summary>
        /// <returns>async</returns>
        Task RollBackAsync();

        /// <summary>
        /// Query command
        /// </summary>
        /// <returns>list of T</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        /// <summary>
        /// Query command
        /// </summary>
        /// <returns>T result or null</returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}

