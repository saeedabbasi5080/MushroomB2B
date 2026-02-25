using MediatR;

namespace MushroomB2B.Application.Features.Admin.Commands.SoftDeleteOrder;

public sealed record SoftDeleteOrderCommand(Guid OrderId) : IRequest<bool>;
