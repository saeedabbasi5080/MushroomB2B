using Microsoft.EntityFrameworkCore.Storage;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Interfaces;

namespace MushroomB2B.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public IGenericRepository<Shop> Shops { get; } =
        new GenericRepository<Shop>(context);

    public IGenericRepository<Order> Orders { get; } =
        new GenericRepository<Order>(context);

    public IGenericRepository<Product> Products { get; } =
        new GenericRepository<Product>(context);

    public IGenericRepository<ProductVariant> ProductVariants { get; } =
        new GenericRepository<ProductVariant>(context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("No active transaction to commit.");

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is null)
            throw new InvalidOperationException("No active transaction to rollback.");

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        context.Dispose();
    }
}
