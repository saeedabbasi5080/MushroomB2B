using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Delivery.Queries.GetDriverBatches;

public sealed record GetDriverBatchesQuery : IRequest<List<DriverBatchDto>>
{
    public required Guid DriverId { get; init; }
    public DateTime? BatchDate { get; init; }
}

public sealed record DriverBatchDto(
    Guid BatchId,
    DateTime BatchDate,
    DeliveryStatus Status,
    List<DeliveryItemDto> Items);

public sealed record DeliveryItemDto(
    Guid DeliveryItemId,
    int SortOrder,
    DeliveryItemStatus Status,
    string ShopName,
    string ShopAddress,
    Guid OrderId,
    decimal TotalAmount);
