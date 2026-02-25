using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.ToTable("Shops");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.OwnerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.IsVerified).IsRequired().HasDefaultValue(false);


        // Address as owned value object — columns: Address_City, Address_Street, etc.
        builder.OwnsOne(s => s.Address, a =>
        {
            a.Property(x => x.City)
                .HasColumnName("Address_City")
                .IsRequired()
                .HasMaxLength(100);

            a.Property(x => x.Street)
                .HasColumnName("Address_Street")
                .IsRequired()
                .HasMaxLength(300);

            a.Property(x => x.GeoLat)
                .HasColumnName("Address_GeoLat");

            a.Property(x => x.GeoLng)
                .HasColumnName("Address_GeoLng");
        });

        builder.Property(s => s.CreditLimit)
            .IsRequired();

        builder.Property(s => s.WalletBalance)
            .IsRequired();

        builder.HasQueryFilter(s => !s.IsDeleted);

        builder.HasMany(s => s.Orders)
            .WithOne()
            .HasForeignKey(o => o.ShopId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
