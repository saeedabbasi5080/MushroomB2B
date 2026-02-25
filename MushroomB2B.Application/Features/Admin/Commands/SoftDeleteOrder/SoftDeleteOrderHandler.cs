using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Admin.Commands.SoftDeleteOrder;

public sealed class SoftDeleteOrderHandler(IAppDbContext db)
    : IRequestHandler<SoftDeleteOrderCommand, bool>
{
    public async Task<bool> Handle(
        SoftDeleteOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new DomainException($"Order '{request.OrderId}' not found.");

        // Restore stock for all order items
        var variantIds = order.Items.Select(i => i.ProductVariantId).ToList();

        var variants = await db.ProductVariants
            .Where(v => variantIds.Contains(v.Id))
            .ToDictionaryAsync(v => v.Id, cancellationToken);

        foreach (var item in order.Items)
        {
            if (!variants.TryGetValue(item.ProductVariantId, out var variant))
                continue;

            variant.RestoreStock(item.Quantity);
            db.ProductVariants.Update(variant);
        }

        // Soft delete order
        order.SoftDelete();

        db.Orders.Update(order);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
