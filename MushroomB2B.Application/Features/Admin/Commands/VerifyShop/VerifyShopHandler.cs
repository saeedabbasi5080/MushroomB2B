using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Admin.Commands.VerifyShop;

public sealed class VerifyShopHandler(IAppDbContext db)
    : IRequestHandler<VerifyShopCommand, VerifyShopResult>
{
    public async Task<VerifyShopResult> Handle(
        VerifyShopCommand request,
        CancellationToken cancellationToken)
    {
        var shop = await db.Shops
            .FirstOrDefaultAsync(s => s.Id == request.ShopId && !s.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Shop '{request.ShopId}' not found.");

        if (shop.IsVerified)
            throw new DomainException($"Shop '{request.ShopId}' is already verified.");

        shop.Verify();

        db.Shops.Update(shop);
        await db.SaveChangesAsync(cancellationToken);

        return new VerifyShopResult(shop.Id, shop.IsVerified);
    }
}
