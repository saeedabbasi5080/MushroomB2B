using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto>;

public sealed record OrderDto(
    Guid Id,
    Guid ShopId,
    OrderStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime? DeliveryDate,
    string? Notes,
    List<OrderItemDto> Items);

public sealed record OrderItemDto(
    Guid ProductVariantId,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal);
