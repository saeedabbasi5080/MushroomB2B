using MediatR;

namespace MushroomB2B.Application.Features.Admin.Queries.GetAllShops;

public sealed record GetAllShopsQuery : IRequest<List<AdminShopDto>>;

public sealed record AdminShopDto(
    Guid ShopId,
    string OwnerName,
    string City,
    decimal WalletBalance,
    decimal CreditLimit,
    bool IsVerified,
    int OrderCount);
