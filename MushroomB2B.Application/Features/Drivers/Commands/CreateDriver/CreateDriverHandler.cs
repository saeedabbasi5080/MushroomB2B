using MediatR;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Application.Features.Drivers.Commands.CreateDriver;

public sealed class CreateDriverHandler(IAppDbContext db)
    : IRequestHandler<CreateDriverCommand, CreateDriverResult>
{
    public async Task<CreateDriverResult> Handle(
        CreateDriverCommand request,
        CancellationToken cancellationToken)
    {
        var driver = new Driver(request.FullName, request.PhoneNumber, request.VehiclePlate);

        await db.Drivers.AddAsync(driver, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return new CreateDriverResult(driver.Id);
    }
}
