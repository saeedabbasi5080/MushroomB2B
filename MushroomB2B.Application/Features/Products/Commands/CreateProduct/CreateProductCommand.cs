using MediatR;

namespace MushroomB2B.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : IRequest<CreateProductResult>
{
    public required string Name { get; init; }
    public required decimal BasePrice { get; init; }
}

public sealed record CreateProductResult(Guid ProductId);
