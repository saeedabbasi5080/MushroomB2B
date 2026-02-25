using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Admin.Commands.SoftDeleteShop;

public sealed class SoftDeleteShopHandler(IAppDbContext db)
    : IRequestHandler<SoftDeleteShopCommand, bool>
{
    public async Task<bool> Handle(
        SoftDeleteShopCommand request,
        CancellationToken cancellationToken)
    {
        var shop = await db.Shops
            .FirstOrDefaultAsync(s => s.Id == request.ShopId, cancellationToken)
            ?? throw new DomainException($"Shop '{request.ShopId}' not found.");

        shop.SoftDelete();

        db.Shops.Update(shop);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
