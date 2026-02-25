using MediatR;

namespace MushroomB2B.Application.Features.Admin.Commands.AssignDeliveryBatch;

public sealed record AssignDeliveryBatchCommand : IRequest<AssignDeliveryBatchResult>
{
    public required Guid DriverId { get; init; }
    public required List<Guid> OrderIds { get; init; }
    public required DateTime BatchDate { get; init; }
}

public sealed record AssignDeliveryBatchResult(Guid BatchId, int AssignedOrderCount);
