using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder.HasKey(v => v.Id);

        // Store enum as string for readability and DB portability
        builder.Property(v => v.Grade)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.WeightUnitKg).IsRequired();
        builder.Property(v => v.StockQuantity).IsRequired();

        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
