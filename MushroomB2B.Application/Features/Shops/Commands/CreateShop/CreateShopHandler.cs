using MediatR;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.ValueObjects;

namespace MushroomB2B.Application.Features.Shops.Commands.CreateShop;

public sealed class CreateShopHandler(IAppDbContext db)
    : IRequestHandler<CreateShopCommand, CreateShopResult>
{
    public async Task<CreateShopResult> Handle(
        CreateShopCommand request,
        CancellationToken cancellationToken)
    {
        //var address = new Address(request.City, request.Street, request.GeoLat, request.GeoLng);
        var address = new Address
        {
            City = request.City,
            Street = request.Street,
            GeoLat = request.GeoLat,
            GeoLng = request.GeoLng
        };

        var shop = new Shop(request.OwnerName, address, request.CreditLimit);

        await db.Shops.AddAsync(shop, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return new CreateShopResult(shop.Id);
    }
}
