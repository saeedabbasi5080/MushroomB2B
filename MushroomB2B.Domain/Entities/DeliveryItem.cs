using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Domain.Entities;

public sealed class DeliveryItem : BaseEntity
{
    public Guid DeliveryBatchId { get; private set; }
    public Guid OrderId { get; private set; }
    public int SortOrder { get; private set; }
    public DeliveryItemStatus Status { get; private set; } = DeliveryItemStatus.Pending;
    public string? FailureReason { get; private set; }
    public DeliveryBatch DeliveryBatch { get; private set; } = default!;

    private DeliveryItem() { }

    public DeliveryItem(Guid deliveryBatchId, Guid orderId, int sortOrder)
    {
        if (deliveryBatchId == Guid.Empty)
            throw new ArgumentException("DeliveryBatchId cannot be empty.", nameof(deliveryBatchId));
        if (orderId == Guid.Empty)
            throw new ArgumentException("OrderId cannot be empty.", nameof(orderId));
        if (sortOrder < 0)
            throw new ArgumentOutOfRangeException(nameof(sortOrder));

        DeliveryBatchId = deliveryBatchId;
        OrderId = orderId;
        SortOrder = sortOrder;
    }

    public void MarkDelivered()
    {
        if (Status != DeliveryItemStatus.Pending)
            throw new DomainException($"DeliveryItem is already in '{Status}' status.");

        Status = DeliveryItemStatus.Delivered;
        SetModified();
    }

    public void MarkFailed(string reason)
    {
        if (Status != DeliveryItemStatus.Pending)
            throw new DomainException($"DeliveryItem is already in '{Status}' status.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Failure reason is required.", nameof(reason));

        Status = DeliveryItemStatus.Failed;
        FailureReason = reason;
        SetModified();
    }
}
