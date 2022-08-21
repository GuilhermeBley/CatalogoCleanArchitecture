using System.Data;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Catalogo.Infrastructure.Connection;

namespace Catalogo.Infrastructure.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool hasConnection = false;
        public bool HasConnection => hasConnection;

        private DbConnection _connection;
        private DbTransaction _transaction;
        private readonly IConnectionFactory _connectionFactory;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CommitAsync()
        {
            ThrowWhenNotCreatedConnection();

            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            ThrowWhenNotCreatedConnection();

            try{
                _transaction?.Dispose();
            }catch{}
            
            try{
                _connection?.Dispose();
            }catch{}
            
            _transaction = null;
            _connection = null;

            hasConnection = false;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            if (!hasConnection)
            {
                _connection = _connectionFactory.CreateConn();
                await _connection.OpenAsync();
                _transaction = await _connection.BeginTransactionAsync();
                hasConnection = true;
            }

            return _connection;
        }

        public async Task RollBackAsync()
        {
            ThrowWhenNotCreatedConnection();

            await _transaction.RollbackAsync();
        }

        private void ThrowWhenNotCreatedConnection()
        {
            if (_connection is null)
            {
                throw new DataException("No has there Connection to execute method.");
            }
        }
    }
}