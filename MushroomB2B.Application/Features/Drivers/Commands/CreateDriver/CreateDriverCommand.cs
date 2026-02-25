using MediatR;

namespace MushroomB2B.Application.Features.Drivers.Commands.CreateDriver;

public sealed record CreateDriverCommand : IRequest<CreateDriverResult>
{
    public required string FullName { get; init; }
    public required string PhoneNumber { get; init; }
    public required string VehiclePlate { get; init; }
}

public sealed record CreateDriverResult(Guid DriverId);
