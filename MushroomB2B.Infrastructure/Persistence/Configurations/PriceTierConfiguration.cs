using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class PriceTierConfiguration : IEntityTypeConfiguration<PriceTier>
{
    public void Configure(EntityTypeBuilder<PriceTier> builder)
    {
        builder.ToTable("PriceTiers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.MinQty).IsRequired();
        builder.Property(t => t.MaxQty);   // nullable
        builder.Property(t => t.DiscountPercentage).IsRequired();

        builder.HasQueryFilter(t => !t.IsDeleted);
    }
}
