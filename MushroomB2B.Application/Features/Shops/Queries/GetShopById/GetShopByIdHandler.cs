using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Shops.Queries.GetShopById;

public sealed class GetShopByIdHandler(IAppDbContext db)
    : IRequestHandler<GetShopByIdQuery, ShopDto>
{
    public async Task<ShopDto> Handle(
        GetShopByIdQuery request,
        CancellationToken cancellationToken)
    {
        var shop = await db.Shops
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.ShopId && !s.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Shop '{request.ShopId}' not found.");

        return new ShopDto(
            shop.Id,
            shop.OwnerName,
            shop.Address.City,
            shop.Address.Street,
            shop.CreditLimit,
            shop.WalletBalance);
    }
}
