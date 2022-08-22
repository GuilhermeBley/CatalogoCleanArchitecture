using System.Data;
using Catalogo.Infrastructure.Context;

namespace Catalogo.Infrastructure.Repositories
{
    /// <summary>
    /// Use this class to get connection to execute the commands.
    /// </summary>
    public class RepositoryBase
    {
        private readonly IUnitOfWorkRepository _uoW;

        /// <summary>
        /// Connection to execute commands and querys
        /// </summary>
        protected IDbConnection _connection => _uoW.Connection;

        /// <summary>
        /// Shared execution
        /// </summary>
        protected IDbTransaction _transaction => _uoW.Transaction;

        public RepositoryBase(IUnitOfWorkRepository uoW)
        {
            _uoW = uoW;
        }
    }
}