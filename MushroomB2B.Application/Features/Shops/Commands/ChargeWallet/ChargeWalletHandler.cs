using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Shops.Commands.ChargeWallet;

public sealed class ChargeWalletHandler(IAppDbContext db)
    : IRequestHandler<ChargeWalletCommand, ChargeWalletResult>
{
    public async Task<ChargeWalletResult> Handle(
        ChargeWalletCommand request,
        CancellationToken cancellationToken)
    {
        var shop = await db.Shops
            .FirstOrDefaultAsync(s => s.Id == request.ShopId && !s.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Shop '{request.ShopId}' not found.");

        shop.ChargeWallet(request.Amount);

        db.Shops.Update(shop);
        await db.SaveChangesAsync(cancellationToken);

        return new ChargeWalletResult(shop.Id, shop.WalletBalance);
    }
}
