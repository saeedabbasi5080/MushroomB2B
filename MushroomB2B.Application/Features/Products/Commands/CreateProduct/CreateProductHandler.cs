using MediatR;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductHandler(IAppDbContext db)
    : IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product(request.Name, request.BasePrice);

        await db.Products.AddAsync(product, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}
