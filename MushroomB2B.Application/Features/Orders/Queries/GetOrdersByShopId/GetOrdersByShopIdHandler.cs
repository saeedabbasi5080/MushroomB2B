using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Common.Models;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Orders.Queries.GetOrdersByShopId;

public sealed class GetOrdersByShopIdHandler(
    IAppDbContext db,
    ICurrentUserService currentUser)
    : IRequestHandler<GetOrdersByShopIdQuery, PaginatedResult<ShopOrderDto>>
{
    public async Task<PaginatedResult<ShopOrderDto>> Handle(
        GetOrdersByShopIdQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Load shop and verify ownership
        var shop = await db.Shops
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.ShopId && !s.IsDeleted, cancellationToken)
            ?? throw new DomainException($"Shop '{request.ShopId}' not found.");

        // 2. Ownership check — Admin can bypass, ShopOwner must own the shop
        var isAdmin = currentUser.Role == "Admin";

        if (!isAdmin)
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId is null)
                throw new DomainException("Access denied.");

            if (shop.UserId != currentUser.UserId)
                throw new DomainException("Access denied. You do not own this shop.");
        }

        // 3. Build base query
        var query = db.Orders
            .AsNoTracking()
            .Where(o => o.ShopId == request.ShopId && !o.IsDeleted);

        if (request.Status.HasValue)
            query = query.Where(o => o.Status == request.Status.Value);

        // 4. Total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // 5. Paginated orders
        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Include(o => o.Items)
            .ToListAsync(cancellationToken);

        // 6. Load variants + products in one query
        var variantIds = orders
            .SelectMany(o => o.Items)
            .Select(i => i.ProductVariantId)
            .Distinct()
            .ToList();

        var variants = await db.ProductVariants
            .AsNoTracking()
            .Include(v => v.Product)
            .Where(v => variantIds.Contains(v.Id))
            .ToDictionaryAsync(v => v.Id, cancellationToken);

        // 7. Map to DTOs
        var items = orders.Select(order => new ShopOrderDto(
            order.Id,
            order.Status,
            order.TotalAmount,
            order.CreatedAt,
            order.DeliveryDate,
            order.Notes,
            order.Items.Select(item =>
            {
                var variant = variants.GetValueOrDefault(item.ProductVariantId);
                return new OrderItemSummaryDto(
                    variant?.Product.Name ?? "Unknown",
                    variant?.Grade.ToString() ?? "Unknown",
                    item.Quantity,
                    variant?.WeightUnitKg ?? 0,
                    item.UnitPrice,
                    item.Subtotal);
            }).ToList()
        )).ToList();

        return new PaginatedResult<ShopOrderDto>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
