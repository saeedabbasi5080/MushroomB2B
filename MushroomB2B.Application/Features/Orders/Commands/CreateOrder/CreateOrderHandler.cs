using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderHandler(IAppDbContext db)
    : IRequestHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Load and validate the shop
        var shop = await db.Shops
            .FirstOrDefaultAsync(s => s.Id == request.ShopId && !s.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Shop '{request.ShopId}' not found.");

        // 2. Create the order aggregate root
        var order = new Order(shop.Id, request.Notes);

        // 3. Process each line item
        foreach (var itemDto in request.Items)
        {
            // 3a. Load variant with its parent product and price tiers
            var variant = await db.ProductVariants
                .Include(v => v.Product)
                    .ThenInclude(p => p.PriceTiers)
                .FirstOrDefaultAsync(
                    v => v.Id == itemDto.ProductVariantId && !v.IsDeleted,
                    cancellationToken)
                ?? throw new DomainException($"ProductVariant '{itemDto.ProductVariantId}' not found.");

            // 3b. Check stock availability
            if (variant.StockQuantity < itemDto.Quantity)
                throw new DomainException(
                    $"Insufficient stock for variant '{itemDto.ProductVariantId}'. " +
                    $"Available: {variant.StockQuantity}, Requested: {itemDto.Quantity}");

            // 3c. Apply tiered pricing — Domain logic on Product aggregate
            var tieredUnitPrice = variant.Product.GetTieredPrice(itemDto.Quantity);

            // 3d. Add item to order (domain method enforces invariants)
            order.AddItem(itemDto.ProductVariantId, itemDto.Quantity, tieredUnitPrice);

            // 3e. Reserve stock immediately
            variant.ReserveStock(itemDto.Quantity);
            db.ProductVariants.Update(variant);
        }

        // 4. Check shop's ability to pay (credit + wallet guard inside domain)
        shop.PlaceOrder(order.TotalAmount);

        // 5. Confirm the order (domain transitions state to Approved)
        order.Confirm();

        // 6. Deduct payment from shop's wallet / credit
        shop.DeductForOrder(order.TotalAmount);

        // 7. Persist everything
        await db.Orders.AddAsync(order, cancellationToken);
        db.Shops.Update(shop);
        await db.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id, order.TotalAmount);
    }
}
