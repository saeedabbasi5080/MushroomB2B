using MediatR;

namespace MushroomB2B.Application.Features.Delivery.Commands.MarkItemDelivered;

public sealed record MarkItemDeliveredCommand : IRequest<bool>
{
    public required Guid DeliveryItemId { get; init; }
    public required Guid DriverId { get; init; }
}
