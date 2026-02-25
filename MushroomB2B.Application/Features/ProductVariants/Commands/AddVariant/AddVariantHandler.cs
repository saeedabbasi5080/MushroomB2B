using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.ProductVariants.Commands.AddVariant;

public sealed class AddVariantHandler(IAppDbContext db)
    : IRequestHandler<AddVariantCommand, AddVariantResult>
{
    public async Task<AddVariantResult> Handle(
        AddVariantCommand request,
        CancellationToken cancellationToken)
    {
        var productExists = await db.Products
            .AnyAsync(p => p.Id == request.ProductId && !p.IsDeleted, cancellationToken);

        if (!productExists)
            throw new DomainException($"Product '{request.ProductId}' not found.");

        var variant = new ProductVariant(
            request.ProductId,
            request.Grade,
            request.WeightUnitKg,
            request.InitialStock);

        await db.ProductVariants.AddAsync(variant, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return new AddVariantResult(variant.Id);
    }
}
