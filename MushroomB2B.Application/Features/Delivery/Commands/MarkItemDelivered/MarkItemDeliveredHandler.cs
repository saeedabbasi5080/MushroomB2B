using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Delivery.Commands.MarkItemDelivered;

public sealed class MarkItemDeliveredHandler(IAppDbContext db)
    : IRequestHandler<MarkItemDeliveredCommand, bool>
{
    public async Task<bool> Handle(
        MarkItemDeliveredCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Load DeliveryItem with its parent batch
        var deliveryItem = await db.DeliveryItems
            .Include(i => i.DeliveryBatch)
            .FirstOrDefaultAsync(i => i.Id == request.DeliveryItemId && !i.IsDeleted, cancellationToken)
            ?? throw new DomainException($"DeliveryItem '{request.DeliveryItemId}' not found.");

        // 2. Security check — verify this item belongs to the requesting driver
        if (deliveryItem.DeliveryBatch.DriverId != request.DriverId)
            throw new DomainException("Access denied. This delivery item does not belong to the specified driver.");

        // 3. Mark delivery item as delivered (domain method enforces state transition)
        deliveryItem.MarkDelivered();

        // 4. Load and update the associated Order
        var order = await db.Orders
            .FirstOrDefaultAsync(o => o.Id == deliveryItem.OrderId && !o.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Order '{deliveryItem.OrderId}' not found.");

        order.MarkAsDelivered();

        db.DeliveryItems.Update(deliveryItem);
        db.Orders.Update(order);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
