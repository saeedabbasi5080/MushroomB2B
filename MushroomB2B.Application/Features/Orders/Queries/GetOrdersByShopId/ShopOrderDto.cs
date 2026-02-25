using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Orders.Queries.GetOrdersByShopId;

public sealed record ShopOrderDto(
    Guid OrderId,
    OrderStatus Status,
    decimal TotalAmount,
    DateTime CreatedAt,
    DateTime? DeliveryDate,
    string? Notes,
    List<OrderItemSummaryDto> Items);

public sealed record OrderItemSummaryDto(
    string ProductName,
    string Grade,
    int Quantity,
    int WeightUnitKg,
    decimal UnitPrice,
    decimal Subtotal);
