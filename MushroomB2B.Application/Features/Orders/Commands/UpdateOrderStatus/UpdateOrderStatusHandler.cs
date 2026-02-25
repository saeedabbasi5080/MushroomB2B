using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Orders.Commands.UpdateOrderStatus;

public sealed class UpdateOrderStatusHandler(IAppDbContext db)
    : IRequestHandler<UpdateOrderStatusCommand, UpdateOrderStatusResult>
{
    public async Task<UpdateOrderStatusResult> Handle(
        UpdateOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await db.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Order '{request.OrderId}' not found.");

        switch (request.NewStatus)
        {
            case OrderStatus.Shipped:
                order.MarkAsShipped();
                break;
            case OrderStatus.Delivered:
                order.MarkAsDelivered();
                break;
            case OrderStatus.Cancelled:
                order.Cancel();
                break;
            default:
                throw new DomainException($"Transition to '{request.NewStatus}' is not allowed via this endpoint.");
        }

        db.Orders.Update(order);
        await db.SaveChangesAsync(cancellationToken);

        return new UpdateOrderStatusResult(order.Id, order.Status);
    }
}
