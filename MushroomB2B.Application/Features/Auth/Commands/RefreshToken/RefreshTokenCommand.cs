using MediatR;
using MushroomB2B.Application.Features.Auth.Commands.VerifyOtp;

namespace MushroomB2B.Application.Features.Auth.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResult>;
