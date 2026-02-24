using MushroomB2B.Domain.Common;

namespace MushroomB2B.Domain.Entities;

public sealed class PriceTier : BaseEntity
{
    public Guid ProductId { get; private set; }
    public int MinQty { get; private set; }
    public int? MaxQty { get; private set; }          // null = no upper bound
    public decimal DiscountPercentage { get; private set; }

    private PriceTier() { }

    public PriceTier(Guid productId, int minQty, int? maxQty, decimal discountPercentage)
    {
        if (minQty <= 0)
            throw new ArgumentOutOfRangeException(nameof(minQty));
        if (maxQty.HasValue && maxQty.Value <= minQty)
            throw new ArgumentException("MaxQty must be greater than MinQty.");
        if (discountPercentage is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(discountPercentage));

        ProductId = productId;
        MinQty = minQty;
        MaxQty = maxQty;
        DiscountPercentage = discountPercentage;
    }
}
