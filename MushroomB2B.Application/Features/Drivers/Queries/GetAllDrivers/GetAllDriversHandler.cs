using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Application.Features.Drivers.Queries.GetAllDrivers;

public sealed class GetAllDriversHandler(IAppDbContext db)
    : IRequestHandler<GetAllDriversQuery, List<DriverDto>>
{
    public async Task<List<DriverDto>> Handle(
        GetAllDriversQuery request,
        CancellationToken cancellationToken)
    {
        return await db.Drivers
            .AsNoTracking()
            .Where(d => !d.IsDeleted)
            .Select(d => new DriverDto(d.Id, d.FullName, d.PhoneNumber, d.VehiclePlate, d.IsActive))
            .OrderBy(d => d.FullName)
            .ToListAsync(cancellationToken);
    }
}
