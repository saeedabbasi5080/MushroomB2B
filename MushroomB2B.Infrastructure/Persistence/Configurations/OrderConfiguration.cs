using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        // Enum stored as string for human-readable DB values
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.TotalAmount).IsRequired();
        builder.Property(o => o.Notes).HasMaxLength(500);
        builder.Property(o => o.DeliveryDate);

        builder.HasQueryFilter(o => !o.IsDeleted);

        // Order → OrderItems (owned collection, private backing field)
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // EF Core accesses the private _items field directly
        builder.Navigation(o => o.Items).HasField("_items");
    }
}
