using MediatR;

namespace MushroomB2B.Application.Features.Drivers.Queries.GetAllDrivers;

public sealed record GetAllDriversQuery : IRequest<List<DriverDto>>;

public sealed record DriverDto(
    Guid Id,
    string FullName,
    string PhoneNumber,
    string VehiclePlate,
    bool IsActive);
