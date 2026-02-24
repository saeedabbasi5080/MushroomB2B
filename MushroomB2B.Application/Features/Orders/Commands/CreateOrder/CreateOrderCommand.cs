using MediatR;

namespace MushroomB2B.Application.Features.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand : IRequest<CreateOrderResult>
{
    public required Guid ShopId { get; init; }
    public required List<OrderItemDto> Items { get; init; }
    public string? Notes { get; init; }
}

public sealed record OrderItemDto
{
    public required Guid ProductVariantId { get; init; }
    public required int Quantity { get; init; }
}

public sealed record CreateOrderResult(Guid OrderId, decimal TotalAmount);
