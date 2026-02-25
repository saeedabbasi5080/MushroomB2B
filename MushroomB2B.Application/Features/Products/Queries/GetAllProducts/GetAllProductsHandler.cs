using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Application.Features.Products.Queries.GetAllProducts;

public sealed class GetAllProductsHandler(IAppDbContext db)
    : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await db.Products
            .AsNoTracking()
            .Include(p => p.PriceTiers)
            .Where(p => !p.IsDeleted)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.BasePrice,
                p.PriceTiers
                    .Where(t => !t.IsDeleted)
                    .Select(t => new PriceTierDto(t.Id, t.MinQty, t.MaxQty, t.DiscountPercentage))
                    .ToList()))
            .ToListAsync(cancellationToken);
    }
}
