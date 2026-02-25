using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.ProductVariants.Commands.AddVariant;

public sealed record AddVariantCommand : IRequest<AddVariantResult>
{
    public required Guid ProductId { get; init; }
    public required MushroomGrade Grade { get; init; }
    public required int WeightUnitKg { get; init; }
    public required int InitialStock { get; init; }
}

public sealed record AddVariantResult(Guid VariantId);
