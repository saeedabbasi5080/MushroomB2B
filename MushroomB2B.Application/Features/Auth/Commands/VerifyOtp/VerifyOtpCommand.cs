using MediatR;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Auth.Commands.VerifyOtp;

public sealed record VerifyOtpCommand : IRequest<AuthResult>
{
    public required string PhoneNumber { get; init; }
    public required string Code { get; init; }
    public UserRole? Role { get; init; }
}

public sealed record AuthResult(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    string Role);
