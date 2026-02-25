using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Products.Commands.AddPriceTier;

public sealed class AddPriceTierHandler(IAppDbContext db)
    : IRequestHandler<AddPriceTierCommand, AddPriceTierResult>
{
    public async Task<AddPriceTierResult> Handle(
        AddPriceTierCommand request,
        CancellationToken cancellationToken)
    {
        var product = await db.Products
            .Include(p => p.PriceTiers)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId && !p.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Product '{request.ProductId}' not found.");

        var tier = new PriceTier(request.ProductId, request.MinQty, request.MaxQty, request.DiscountPercentage);
        product.AddPriceTier(tier);

        db.Products.Update(product);
        await db.SaveChangesAsync(cancellationToken);

        return new AddPriceTierResult(tier.Id);
    }
}
