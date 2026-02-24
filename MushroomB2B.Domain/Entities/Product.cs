using MushroomB2B.Domain.Common;

namespace MushroomB2B.Domain.Entities;

public sealed class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal BasePrice { get; private set; }

    private readonly List<ProductVariant> _variants = [];
    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    private readonly List<PriceTier> _priceTiers = [];
    public IReadOnlyCollection<PriceTier> PriceTiers => _priceTiers.AsReadOnly();

    private Product() { Name = string.Empty; }

    public Product(string name, decimal basePrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));
        if (basePrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(basePrice), "Base price must be positive.");

        Name = name;
        BasePrice = basePrice;
    }

    public void AddVariant(ProductVariant variant)
    {
        ArgumentNullException.ThrowIfNull(variant);
        _variants.Add(variant);
        SetModified();
    }

    public void AddPriceTier(PriceTier tier)
    {
        ArgumentNullException.ThrowIfNull(tier);
        _priceTiers.Add(tier);
        SetModified();
    }

    /// <summary>
    /// Resolves the applicable discount for a given quantity.
    /// Returns the final unit price after applying the best matching tier discount.
    /// </summary>
    public decimal GetTieredPrice(int quantity)
    {
        var applicableTier = _priceTiers
            .Where(t => quantity >= t.MinQty && (t.MaxQty == null || quantity <= t.MaxQty))
            .OrderByDescending(t => t.DiscountPercentage)
            .FirstOrDefault();

        if (applicableTier is null)
            return BasePrice;

        return BasePrice * (1 - applicableTier.DiscountPercentage / 100m);
    }
}
