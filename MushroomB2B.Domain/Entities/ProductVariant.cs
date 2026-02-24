using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Domain.Entities;

public sealed class ProductVariant : BaseEntity
{
    public Guid ProductId { get; private set; }
    public MushroomGrade Grade { get; private set; }
    public int WeightUnitKg { get; private set; }   // 5 or 10
    public int StockQuantity { get; private set; }

    private ProductVariant() { }

    public ProductVariant(Guid productId, MushroomGrade grade, int weightUnitKg, int initialStock)
    {
        if (weightUnitKg is not (5 or 10))
            throw new ArgumentException("Weight unit must be either 5kg or 10kg.", nameof(weightUnitKg));
        if (initialStock < 0)
            throw new ArgumentOutOfRangeException(nameof(initialStock));

        ProductId = productId;
        Grade = grade;
        WeightUnitKg = weightUnitKg;
        StockQuantity = initialStock;
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));
        if (StockQuantity < quantity)
            throw new DomainException(
                $"Insufficient stock for variant {Id}. Requested: {quantity}, Available: {StockQuantity}");

        StockQuantity -= quantity;
        SetModified();
    }

    public void RestoreStock(int quantity)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        StockQuantity += quantity;
        SetModified();
    }
}
