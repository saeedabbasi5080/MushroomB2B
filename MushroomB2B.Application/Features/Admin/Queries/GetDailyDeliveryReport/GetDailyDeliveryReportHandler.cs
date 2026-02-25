using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Admin.Queries.GetDailyDeliveryReport;

public sealed class GetDailyDeliveryReportHandler(IAppDbContext db)
    : IRequestHandler<GetDailyDeliveryReportQuery, List<DeliveryReportItemDto>>
{
    public async Task<List<DeliveryReportItemDto>> Handle(
        GetDailyDeliveryReportQuery request,
        CancellationToken cancellationToken)
    {
        var targetDate = request.TargetDate.Date;

        // Load approved orders for the target date with all related data
        var orders = await db.Orders
            .AsNoTracking()
            .Where(o => !o.IsDeleted
                && o.Status == OrderStatus.Approved
                && o.CreatedAt.Date == targetDate)
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);

        if (orders.Count == 0)
            return [];

        var shopIds = orders.Select(o => o.ShopId).Distinct().ToList();
        var variantIds = orders
            .SelectMany(o => o.Items)
            .Select(i => i.ProductVariantId)
            .Distinct()
            .ToList();

        var shops = await db.Shops
            .AsNoTracking()
            .Where(s => shopIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, cancellationToken);

        var variants = await db.ProductVariants
            .AsNoTracking()
            .Include(v => v.Product)
            .Where(v => variantIds.Contains(v.Id))
            .ToDictionaryAsync(v => v.Id, cancellationToken);

        return orders.Select(order =>
        {
            var shop = shops[order.ShopId];

            var lines = order.Items.Select(item =>
            {
                var variant = variants[item.ProductVariantId];
                return new DeliveryReportLineDto(
                    variant.Product.Name,
                    variant.Grade.ToString(),
                    item.Quantity,
                    variant.WeightUnitKg);
            }).ToList();

            var totalWeightKg = lines.Sum(l => l.Quantity * l.WeightUnitKg);

            return new DeliveryReportItemDto(
                order.Id,
                shop.OwnerName,
                shop.Address.ToString(),
                totalWeightKg,
                lines);
        }).ToList();
    }
}
