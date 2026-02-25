using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Orders.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand : IRequest<UpdateOrderStatusResult>
{
    public required Guid OrderId { get; init; }
    public required OrderStatus NewStatus { get; init; }
}

public sealed record UpdateOrderStatusResult(Guid OrderId, OrderStatus Status);
