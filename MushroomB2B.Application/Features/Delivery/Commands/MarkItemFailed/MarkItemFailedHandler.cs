using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Delivery.Commands.MarkItemFailed;

public sealed class MarkItemFailedHandler(IAppDbContext db)
    : IRequestHandler<MarkItemFailedCommand, bool>
{
    public async Task<bool> Handle(
        MarkItemFailedCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Load DeliveryItem with its parent batch
        var deliveryItem = await db.DeliveryItems
            .Include(i => i.DeliveryBatch)
            .FirstOrDefaultAsync(i => i.Id == request.DeliveryItemId && !i.IsDeleted, cancellationToken)
            ?? throw new DomainException($"DeliveryItem '{request.DeliveryItemId}' not found.");

        // 2. Security check — verify driver ownership
        if (deliveryItem.DeliveryBatch.DriverId != request.DriverId)
            throw new DomainException("Access denied. This delivery item does not belong to the specified driver.");

        // 3. Mark delivery item as failed
        deliveryItem.MarkFailed(request.FailureReason);

        // 4. Load the order with its items to restore stock
        var order = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == deliveryItem.OrderId && !o.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Order '{deliveryItem.OrderId}' not found.");

        // 5. Cancel the order (domain method enforces valid transitions)
        order.Cancel();

        // 6. Restore stock for every order item
        var variantIds = order.Items.Select(i => i.ProductVariantId).ToList();

        var variants = await db.ProductVariants
            .Where(v => variantIds.Contains(v.Id))
            .ToDictionaryAsync(v => v.Id, cancellationToken);

        foreach (var item in order.Items)
        {
            if (!variants.TryGetValue(item.ProductVariantId, out var variant))
                throw new DomainException($"ProductVariant '{item.ProductVariantId}' not found during stock restore.");

            variant.RestoreStock(item.Quantity);
            db.ProductVariants.Update(variant);
        }

        db.DeliveryItems.Update(deliveryItem);
        db.Orders.Update(order);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
