using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Features.Auth.Commands.VerifyOtp;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Auth.Commands.RefreshToken;

public sealed class RefreshTokenHandler(IAppDbContext db, IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    public async Task<AuthResult> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Find user by refresh token
        var user = await db.Users
            .FirstOrDefaultAsync(
                u => u.RefreshToken == request.RefreshToken && !u.IsDeleted,
                cancellationToken)
            ?? throw new DomainException("Invalid refresh token.");

        // 2. Validate expiry
        if (user.RefreshTokenExpiresAt is null || user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new DomainException("Refresh token has expired. Please log in again.");

        // 3. Rotate tokens
        var newAccessToken = jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role);
        var newRefreshToken = Guid.NewGuid().ToString("N");
        var newRefreshExpiry = DateTime.UtcNow.AddDays(7);

        user.SetRefreshToken(newRefreshToken, newRefreshExpiry);

        db.Users.Update(user);
        await db.SaveChangesAsync(cancellationToken);

        return new AuthResult(
            newAccessToken,
            newRefreshToken,
            DateTime.UtcNow.AddMinutes(15),
            user.Role.ToString());
    }
}
