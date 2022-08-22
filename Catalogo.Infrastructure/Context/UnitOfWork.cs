using Dapper;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Catalogo.Infrastructure.Connection;
using System.Collections.Generic;

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

        public void Dispose()
        {
            if (_transaction is not null)
                _transaction?.Dispose();
            
            if (_connection is not null)
                _connection?.Dispose();
            
            _transaction = null;
            _connection = null;

            hasConnection = false;
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ThrowWhenNotCreatedConnection();

            return await _connection.ExecuteAsync(sql, param, _transaction, commandTimeout, commandType);
        }

        public async Task RollBackAsync()
        {
            ThrowWhenNotCreatedConnection();

            await _transaction.RollbackAsync();
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ThrowWhenNotCreatedConnection();

            return await _connection.QueryAsync<T>(sql, param, _transaction, commandTimeout, commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            ThrowWhenNotCreatedConnection();

            return await _connection.QueryFirstOrDefaultAsync<T>(sql, param, _transaction, commandTimeout, commandType);
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