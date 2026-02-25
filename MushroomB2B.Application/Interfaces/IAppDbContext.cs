using Microsoft.EntityFrameworkCore;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Shop> Shops { get; }
    DbSet<Product> Products { get; }
    DbSet<ProductVariant> ProductVariants { get; }
    DbSet<PriceTier> PriceTiers { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
    DbSet<Driver> Drivers { get; }
    DbSet<DeliveryBatch> DeliveryBatches { get; }
    DbSet<DeliveryItem> DeliveryItems { get; }
    DbSet<User> Users { get; }
    DbSet<OtpCode> OtpCodes { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
