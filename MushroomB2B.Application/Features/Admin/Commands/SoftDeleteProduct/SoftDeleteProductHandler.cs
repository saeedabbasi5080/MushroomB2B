using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Admin.Commands.SoftDeleteProduct;

public sealed class SoftDeleteProductHandler(IAppDbContext db)
    : IRequestHandler<SoftDeleteProductCommand, bool>
{
    public async Task<bool> Handle(
        SoftDeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await db.Products
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken)
            ?? throw new DomainException($"Product '{request.ProductId}' not found.");

        product.SoftDelete();

        db.Products.Update(product);
        await db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
