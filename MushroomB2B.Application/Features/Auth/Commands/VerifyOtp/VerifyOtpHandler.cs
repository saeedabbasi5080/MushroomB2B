//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using MushroomB2B.Application.Interfaces;
//using MushroomB2B.Domain.Entities;
//using MushroomB2B.Domain.Enums;
//using MushroomB2B.Domain.Exceptions;

//namespace MushroomB2B.Application.Features.Auth.Commands.VerifyOtp;

//public sealed class VerifyOtpHandler(IAppDbContext db, IJwtService jwtService)
//    : IRequestHandler<VerifyOtpCommand, AuthResult>
//{
//    public async Task<AuthResult> Handle(
//        VerifyOtpCommand request,
//        CancellationToken cancellationToken)
//    {
//        // 1. Admin role can never be self-assigned
//        if (request.Role == UserRole.Admin)
//            throw new DomainException("Admin role cannot be self-assigned.");

//        // 2. Find and verify OTP
//        var otpCode = await db.OtpCodes
//            .Where(o => o.PhoneNumber == request.PhoneNumber
//                && !o.IsUsed
//                && o.ExpiresAt > DateTime.UtcNow)
//            .OrderByDescending(o => o.CreatedAt)
//            .FirstOrDefaultAsync(cancellationToken)
//            ?? throw new DomainException("No valid OTP found. Please request a new one.");

//        otpCode.Verify(request.Code);
//        otpCode.MarkAsUsed();

//        // 3. Find or create user
//        var user = await db.Users
//            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

//        if (user is null)
//        {
//            // New user — assign requested role (default: ShopOwner)
//            var assignedRole = request.Role switch
//            {
//                UserRole.Driver => UserRole.Driver,
//                _ => UserRole.ShopOwner
//            };

//            user = new User(request.PhoneNumber, assignedRole);
//            await db.Users.AddAsync(user, cancellationToken);
//        }
//        // Existing user — role is never changed via OTP flow

//        // 4. Generate tokens
//        var accessToken = jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role);
//        var refreshToken = Guid.NewGuid().ToString("N");

//        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

//        db.OtpCodes.Update(otpCode);
//        await db.SaveChangesAsync(cancellationToken);

//        return new AuthResult(
//            accessToken,
//            refreshToken,
//            DateTime.UtcNow.AddMinutes(15),
//            user.Role.ToString());
//    }
//}


using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Auth.Commands.VerifyOtp;

public sealed class VerifyOtpHandler(IAppDbContext db, IJwtService jwtService)
    : IRequestHandler<VerifyOtpCommand, AuthResult>
{
    public async Task<AuthResult> Handle(
        VerifyOtpCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Admin role can never be self-assigned
        if (request.Role == UserRole.Admin)
            throw new DomainException("Admin role cannot be self-assigned.");

        // 2. Hardcoded OTP validation (temporary)
        if (request.Code != "123456")
            throw new DomainException("Invalid OTP code.");

        // 3. Find or create user
        var user = await db.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);

        if (user is null)
        {
            // New user — assign requested role (default: ShopOwner)
            var assignedRole = request.Role switch
            {
                UserRole.Driver => UserRole.Driver,
                _ => UserRole.ShopOwner
            };

            user = new User(request.PhoneNumber, assignedRole);
            await db.Users.AddAsync(user, cancellationToken);
        }
        // Existing user — role is never changed via OTP flow

        // 4. Generate tokens
        var accessToken = jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role);
        var refreshToken = Guid.NewGuid().ToString("N");

        user.SetRefreshToken(refreshToken, DateTime.UtcNow.AddDays(7));

        await db.SaveChangesAsync(cancellationToken);

        return new AuthResult(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(15),
            user.Role.ToString());
    }
}
