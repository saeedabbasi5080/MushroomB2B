using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Auth.Commands.RequestOtp;

public sealed record RequestOtpCommand : IRequest<bool>
{
    public required string PhoneNumber { get; init; }
    public UserRole? Role { get; init; }
}
