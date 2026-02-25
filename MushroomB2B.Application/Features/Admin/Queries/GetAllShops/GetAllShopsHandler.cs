using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Application.Features.Admin.Queries.GetAllShops;

public sealed class GetAllShopsHandler(IAppDbContext db)
    : IRequestHandler<GetAllShopsQuery, List<AdminShopDto>>
{
    public async Task<List<AdminShopDto>> Handle(
        GetAllShopsQuery request,
        CancellationToken cancellationToken)
    {
        return await db.Shops
            .AsNoTracking()
            .Where(s => !s.IsDeleted)
            .Select(s => new AdminShopDto(
                s.Id,
                s.OwnerName,
                s.Address.City,
                s.WalletBalance,
                s.CreditLimit,
                s.IsVerified,
                db.Orders.Count(o => o.ShopId == s.Id && !o.IsDeleted)))
            .OrderBy(s => s.OwnerName)
            .ToListAsync(cancellationToken);
    }
}
