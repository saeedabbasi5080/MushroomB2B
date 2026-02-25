using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Application.Features.Admin.Queries.GetAllOrders;

public sealed class GetAllOrdersHandler(IAppDbContext db)
    : IRequestHandler<GetAllOrdersQuery, List<AdminOrderDto>>
{
    public async Task<List<AdminOrderDto>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.Orders
            .AsNoTracking()
            .Where(o => !o.IsDeleted);

        if (request.Status.HasValue)
            query = query.Where(o => o.Status == request.Status.Value);

        if (request.ShopId.HasValue)
            query = query.Where(o => o.ShopId == request.ShopId.Value);

        if (request.FromDate.HasValue)
            query = query.Where(o => o.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(o => o.CreatedAt <= request.ToDate.Value);

        return await query
            .Join(db.Shops,
                order => order.ShopId,
                shop => shop.Id,
                (order, shop) => new AdminOrderDto(
                    order.Id,
                    shop.OwnerName,
                    order.TotalAmount,
                    order.Status,
                    order.CreatedAt))
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
