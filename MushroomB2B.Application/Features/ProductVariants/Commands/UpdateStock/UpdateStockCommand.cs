using MediatR;

namespace MushroomB2B.Application.Features.ProductVariants.Commands.UpdateStock;

public sealed record UpdateStockCommand : IRequest<UpdateStockResult>
{
    public required Guid VariantId { get; init; }
    public required int Quantity { get; init; }  // مثبت = اضافه، منفی = کم
}

public sealed record UpdateStockResult(Guid VariantId, int NewStockQuantity);
