using Microsoft.EntityFrameworkCore.Storage;
using Shazam.Application.Interfaces;
using Shazam.Persistence.Context;

namespace Shazam.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShazamContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ShazamContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync(ct);
            return _transaction;
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(ct);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
