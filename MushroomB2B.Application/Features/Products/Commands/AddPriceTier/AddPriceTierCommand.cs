using MediatR;

namespace MushroomB2B.Application.Features.Products.Commands.AddPriceTier;

public sealed record AddPriceTierCommand : IRequest<AddPriceTierResult>
{
    public required Guid ProductId { get; init; }
    public required int MinQty { get; init; }
    public int? MaxQty { get; init; }
    public required decimal DiscountPercentage { get; init; }
}

public sealed record AddPriceTierResult(Guid PriceTierId);
