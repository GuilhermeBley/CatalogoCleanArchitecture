using Catalogo.Infrastructure.Connection;
using Catalogo.Application.UoW;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Catalogo.Infrastructure.Context
{
    public class UnitOfWork : IUnitOfWorkRepository
    {
        private DbConnection _connection { get; set; }
        public DbTransaction _transaction { get; set; }

        public IDbConnection Connection => _connection ?? throw new DataException("Connection is closed.");

        public IDbTransaction Transaction => _transaction;

        private readonly IConnectionFactory _connectionFactory;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IUnitOfWork> BeginTransactionAsync()
        {
            if (_transaction is null)
                _transaction = await  _connection.BeginTransactionAsync();

            return this;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _transaction?.CommitAsync();
            }
            catch
            {
                await _transaction?.RollbackAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            if (_transaction is not null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection is not null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public async Task<IUnitOfWork> OpenConnectionAsync()
        {
            if (_connection is null)
            {
                _connection = _connectionFactory.CreateConn();
                await _connection.OpenAsync();
            }

            return this;
        }
    }
}