using MediatR;

namespace MushroomB2B.Application.Features.Shops.Queries.GetShopById;

public sealed record GetShopByIdQuery(Guid ShopId) : IRequest<ShopDto>;

public sealed record ShopDto(
    Guid Id,
    string OwnerName,
    string City,
    string Street,
    decimal CreditLimit,
    decimal WalletBalance);
