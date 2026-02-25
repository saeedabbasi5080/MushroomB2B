using MediatR;

namespace MushroomB2B.Application.Features.Shops.Commands.CreateShop;

public sealed record CreateShopCommand : IRequest<CreateShopResult>
{
    public required string OwnerName { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public double? GeoLat { get; init; }
    public double? GeoLng { get; init; }
    public decimal CreditLimit { get; init; }
}

public sealed record CreateShopResult(Guid ShopId);
