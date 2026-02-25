using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Domain.Entities;

public sealed class DeliveryBatch : BaseEntity
{
    public Guid DriverId { get; private set; }
    public DateTime BatchDate { get; private set; }
    public DeliveryStatus Status { get; private set; } = DeliveryStatus.Pending;

    private readonly List<DeliveryItem> _items = [];
    public IReadOnlyCollection<DeliveryItem> Items => _items.AsReadOnly();

    private DeliveryBatch() { }

    public DeliveryBatch(Guid driverId, DateTime batchDate)
    {
        if (driverId == Guid.Empty)
            throw new ArgumentException("DriverId cannot be empty.", nameof(driverId));

        DriverId = driverId;
        BatchDate = batchDate.Date;
    }

    public void AssignOrder(Guid orderId, int sortOrder)
    {
        if (Status != DeliveryStatus.Pending)
            throw new DomainException("Orders can only be assigned to a Pending batch.");

        if (_items.Any(i => i.OrderId == orderId))
            throw new DomainException($"Order '{orderId}' is already in this batch.");

        _items.Add(new DeliveryItem(Id, orderId, sortOrder));
        SetModified();
    }

    public void MarkInProgress()
    {
        if (Status != DeliveryStatus.Pending)
            throw new DomainException($"Batch must be Pending to start. Current: '{Status}'.");
        if (_items.Count == 0)
            throw new DomainException("Cannot start a batch with no orders.");

        Status = DeliveryStatus.InProgress;
        SetModified();
    }

    public void MarkCompleted()
    {
        if (Status != DeliveryStatus.InProgress)
            throw new DomainException($"Batch must be InProgress to complete. Current: '{Status}'.");

        Status = DeliveryStatus.Completed;
        SetModified();
    }
}
