using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class OtpCodeConfiguration : IEntityTypeConfiguration<OtpCode>
{
    public void Configure(EntityTypeBuilder<OtpCode> builder)
    {
        builder.ToTable("OtpCodes");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(o => o.Code)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(o => o.ExpiresAt).IsRequired();
        builder.Property(o => o.IsUsed).IsRequired();

        builder.HasQueryFilter(o => !o.IsDeleted);

        builder.HasIndex(o => o.PhoneNumber);
    }
}
