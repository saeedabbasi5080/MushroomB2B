using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class DeliveryItemConfiguration : IEntityTypeConfiguration<DeliveryItem>
{
    public void Configure(EntityTypeBuilder<DeliveryItem> builder)
    {
        builder.ToTable("DeliveryItems");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.SortOrder).IsRequired();
        builder.Property(i => i.FailureReason).HasMaxLength(500);

        builder.HasQueryFilter(i => !i.IsDeleted);

        builder.HasOne(i => i.DeliveryBatch)
    .WithMany(b => b.Items)
    .HasForeignKey(i => i.DeliveryBatchId);

    }
}
