using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Delivery.Queries.GetDriverBatches;

public sealed class GetDriverBatchesHandler(IAppDbContext db)
    : IRequestHandler<GetDriverBatchesQuery, List<DriverBatchDto>>
{
    public async Task<List<DriverBatchDto>> Handle(
        GetDriverBatchesQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Validate driver exists
        var driverExists = await db.Drivers
            .AnyAsync(d => d.Id == request.DriverId && !d.IsDeleted, cancellationToken);

        if (!driverExists)
            throw new DomainException($"Driver '{request.DriverId}' not found.");

        // 2. Build base query
        var targetDate = (request.BatchDate ?? DateTime.UtcNow).Date;

        var batches = await db.DeliveryBatches
            .AsNoTracking()
            .Where(b => b.DriverId == request.DriverId
                && !b.IsDeleted
                && b.BatchDate.Date == targetDate)
            .Include(b => b.Items)
            .OrderBy(b => b.BatchDate)
            .ToListAsync(cancellationToken);

        if (batches.Count == 0)
            return [];

        // 3. Collect all OrderIds and load Orders + Shops in two queries
        var orderIds = batches
            .SelectMany(b => b.Items)
            .Select(i => i.OrderId)
            .Distinct()
            .ToList();

        var orders = await db.Orders
            .AsNoTracking()
            .Where(o => orderIds.Contains(o.Id))
            .ToDictionaryAsync(o => o.Id, cancellationToken);

        var shopIds = orders.Values.Select(o => o.ShopId).Distinct().ToList();

        var shops = await db.Shops
            .AsNoTracking()
            .Where(s => shopIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, cancellationToken);

        // 4. Map to DTOs
        return batches.Select(batch => new DriverBatchDto(
            batch.Id,
            batch.BatchDate,
            batch.Status,
            batch.Items
                .OrderBy(i => i.SortOrder)
                .Select(item =>
                {
                    var order = orders.GetValueOrDefault(item.OrderId);
                    var shop = order is not null
                        ? shops.GetValueOrDefault(order.ShopId)
                        : null;

                    return new DeliveryItemDto(
                        item.Id,
                        item.SortOrder,
                        item.Status,
                        shop?.OwnerName ?? "Unknown",
                        shop?.Address.ToString() ?? "Unknown",
                        item.OrderId,
                        order?.TotalAmount ?? 0);
                }).ToList()
        )).ToList();
    }
}
