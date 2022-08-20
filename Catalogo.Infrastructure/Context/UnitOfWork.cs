using System.Data;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Catalogo.Infrastructure.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool IsOpen { get; set; } = false;
        private bool IsDisposed { get; set; } = false;
        private readonly DbConnection _connection;
        private DbTransaction Transaction;

        public UnitOfWork(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task CommitAsync()
        {
            ThrowIfDisposed();

            await Transaction.CommitAsync();
        }

        public void Dispose()
        {
            ThrowIfDisposed();

            Transaction?.Dispose();
            _connection?.Dispose();

            IsDisposed = true;
        }

        public async Task<IDbConnection> GetConn()
        {
            ThrowIfDisposed();

            if (!IsOpen)
            {
                await _connection.OpenAsync();
                Transaction = await _connection.BeginTransactionAsync();
                IsOpen = true;
            }

            return _connection;
        }

        public async Task RollBackAsync()
        {
            ThrowIfDisposed();

            await Transaction.RollbackAsync();
        }

        private void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));
        }
    }
}