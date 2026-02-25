using MediatR;
using MushroomB2B.Application.Common.Models;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Orders.Queries.GetOrdersByShopId;

public sealed record GetOrdersByShopIdQuery : IRequest<PaginatedResult<ShopOrderDto>>
{
    public required Guid ShopId { get; init; }
    public OrderStatus? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
