using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Domain.Entities;

public sealed class Order : BaseEntity
{
    public Guid ShopId { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; private set; }
    public DateTime? DeliveryDate { get; private set; }
    public string? Notes { get; private set; }

    private readonly List<OrderItem> _items = [];
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public Order(Guid shopId, string? notes = null)
    {
        if (shopId == Guid.Empty)
            throw new ArgumentException("ShopId cannot be empty.", nameof(shopId));

        ShopId = shopId;
        Notes = notes;
        Status = OrderStatus.Pending;
    }

    public void AddItem(Guid productVariantId, int quantity, decimal unitPrice)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Items can only be added to a Pending order.");

        var existingItem = _items.FirstOrDefault(i => i.ProductVariantId == productVariantId);
        if (existingItem is not null)
            throw new DomainException($"Variant {productVariantId} is already in the order. Update quantity instead.");

        var item = new OrderItem(Id, productVariantId, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotal();
        SetModified();
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Cannot confirm an order in '{Status}' status.");
        if (_items.Count == 0)
            throw new DomainException("Cannot confirm an order with no items.");

        Status = OrderStatus.Approved;
        SetModified();
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.Approved)
            throw new DomainException($"Only Approved orders can be shipped. Current: '{Status}'.");

        Status = OrderStatus.Shipped;
        SetModified();
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainException($"Only Shipped orders can be marked Delivered. Current: '{Status}'.");

        Status = OrderStatus.Delivered;
        DeliveryDate = DateTime.UtcNow;
        SetModified();
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Delivered or OrderStatus.Cancelled)
            throw new DomainException($"Order in '{Status}' status cannot be cancelled.");

        Status = OrderStatus.Cancelled;
        SetModified();
    }

    private void RecalculateTotal() =>
        TotalAmount = _items.Sum(i => i.Subtotal);
}
