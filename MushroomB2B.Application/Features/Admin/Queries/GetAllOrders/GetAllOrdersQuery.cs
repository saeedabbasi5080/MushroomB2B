using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Admin.Queries.GetAllOrders;

public sealed record GetAllOrdersQuery : IRequest<List<AdminOrderDto>>
{
    public OrderStatus? Status { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public Guid? ShopId { get; init; }
}

public sealed record AdminOrderDto(
    Guid OrderId,
    string ShopName,
    decimal TotalAmount,
    OrderStatus Status,
    DateTime CreatedAt);
