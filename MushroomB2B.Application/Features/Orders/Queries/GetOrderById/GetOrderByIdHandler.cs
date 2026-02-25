using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdHandler(IAppDbContext db)
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await db.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Order '{request.OrderId}' not found.");

        return new OrderDto(
            order.Id,
            order.ShopId,
            order.Status,
            order.TotalAmount,
            order.CreatedAt,
            order.DeliveryDate,
            order.Notes,
            order.Items.Select(i => new OrderItemDto(
                i.ProductVariantId,
                i.Quantity,
                i.UnitPrice,
                i.Subtotal)).ToList());
    }
}
