using MediatR;

namespace MushroomB2B.Application.Features.Delivery.Commands.MarkItemFailed;

public sealed record MarkItemFailedCommand : IRequest<bool>
{
    public required Guid DeliveryItemId { get; init; }
    public required Guid DriverId { get; init; }
    public required string FailureReason { get; init; }
}
