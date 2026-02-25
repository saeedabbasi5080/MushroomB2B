using MediatR;

namespace MushroomB2B.Application.Features.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery : IRequest<List<ProductDto>>;

public sealed record ProductDto(
    Guid Id,
    string Name,
    decimal BasePrice,
    List<PriceTierDto> PriceTiers);

public sealed record PriceTierDto(
    Guid Id,
    int MinQty,
    int? MaxQty,
    decimal DiscountPercentage);
