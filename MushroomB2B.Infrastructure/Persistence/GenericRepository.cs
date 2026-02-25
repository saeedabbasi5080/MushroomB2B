using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Interfaces;

namespace MushroomB2B.Infrastructure.Persistence;

public sealed class GenericRepository<TEntity>(AppDbContext context)
    : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _set = context.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _set.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _set.Where(x => !x.IsDeleted).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await _set.Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _set.AddAsync(entity, cancellationToken);

    public void Update(TEntity entity)
        => _set.Update(entity);

    // Soft delete only — never hard delete
    public void Remove(TEntity entity)
    {
        entity.SoftDelete();
        _set.Update(entity);
    }
}
