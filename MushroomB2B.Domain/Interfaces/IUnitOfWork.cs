using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Shop> Shops { get; }
    IGenericRepository<Order> Orders { get; }
    IGenericRepository<Product> Products { get; }
    IGenericRepository<ProductVariant> ProductVariants { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
