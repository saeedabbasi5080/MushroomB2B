using MushroomB2B.Domain.Common;

namespace MushroomB2B.Domain.Entities;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }    // Locked-in price at order time
    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(Guid orderId, Guid productVariantId, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));
        if (unitPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(unitPrice));

        OrderId = orderId;
        ProductVariantId = productVariantId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
