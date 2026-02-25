using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.ProductVariants.Commands.UpdateStock;

public sealed class UpdateStockHandler(IAppDbContext db)
    : IRequestHandler<UpdateStockCommand, UpdateStockResult>
{
    public async Task<UpdateStockResult> Handle(
        UpdateStockCommand request,
        CancellationToken cancellationToken)
    {
        var variant = await db.ProductVariants
            .FirstOrDefaultAsync(v => v.Id == request.VariantId && !v.IsDeleted, cancellationToken)
            ?? throw new DomainException($"ProductVariant '{request.VariantId}' not found.");

        if (request.Quantity > 0)
            variant.RestoreStock(request.Quantity);
        else if (request.Quantity < 0)
            variant.ReserveStock(Math.Abs(request.Quantity));
        else
            throw new DomainException("Quantity cannot be zero.");

        db.ProductVariants.Update(variant);
        await db.SaveChangesAsync(cancellationToken);

        return new UpdateStockResult(variant.Id, variant.StockQuantity);
    }
}
