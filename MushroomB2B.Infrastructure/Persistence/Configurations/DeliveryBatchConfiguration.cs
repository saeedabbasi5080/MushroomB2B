using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class DeliveryBatchConfiguration : IEntityTypeConfiguration<DeliveryBatch>
{
    public void Configure(EntityTypeBuilder<DeliveryBatch> builder)
    {
        builder.ToTable("DeliveryBatches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BatchDate).IsRequired();

        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasQueryFilter(b => !b.IsDeleted);

        builder.HasOne<Driver>()
            .WithMany()
            .HasForeignKey(b => b.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(b => b.Items)
            .WithOne()
            .HasForeignKey(i => i.DeliveryBatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(b => b.Items).HasField("_items");
    }
}
