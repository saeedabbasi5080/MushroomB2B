using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Admin.Commands.AssignDeliveryBatch;

public sealed class AssignDeliveryBatchHandler(IAppDbContext db)
    : IRequestHandler<AssignDeliveryBatchCommand, AssignDeliveryBatchResult>
{
    public async Task<AssignDeliveryBatchResult> Handle(
        AssignDeliveryBatchCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate driver exists and is active
        var driver = await db.Drivers
            .FirstOrDefaultAsync(d => d.Id == request.DriverId && !d.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Driver '{request.DriverId}' not found.");

        if (!driver.IsActive)
            throw new DomainException($"Driver '{request.DriverId}' is not active.");

        // 2. Validate all orders exist and are in Approved status
        var orders = await db.Orders
            .Where(o => request.OrderIds.Contains(o.Id) && !o.IsDeleted)
            .ToListAsync(cancellationToken);

        if (orders.Count != request.OrderIds.Count)
        {
            var foundIds = orders.Select(o => o.Id).ToHashSet();
            var missing = request.OrderIds.Where(id => !foundIds.Contains(id));
            throw new DomainException($"Orders not found: {string.Join(", ", missing)}");
        }

        var nonApproved = orders.Where(o => o.Status != OrderStatus.Approved).ToList();
        if (nonApproved.Count > 0)
            throw new DomainException(
                $"Orders must be in Approved status. Invalid orders: {string.Join(", ", nonApproved.Select(o => o.Id))}");

        // 3. Create batch and assign orders with sort order
        var batch = new DeliveryBatch(request.DriverId, request.BatchDate);

        for (var i = 0; i < request.OrderIds.Count; i++)
            batch.AssignOrder(request.OrderIds[i], i + 1);

        await db.DeliveryBatches.AddAsync(batch, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return new AssignDeliveryBatchResult(batch.Id, batch.Items.Count);
    }
}
