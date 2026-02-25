using MediatR;
using Microsoft.EntityFrameworkCore;
using MushroomB2B.Application.Interfaces;
using MushroomB2B.Domain.Entities;
using MushroomB2B.Domain.Enums;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Application.Features.Auth.Commands.RequestOtp;

public sealed class RequestOtpHandler(IAppDbContext db, ISmsService smsService)
    : IRequestHandler<RequestOtpCommand, bool>
{
    public async Task<bool> Handle(
        RequestOtpCommand request,
        CancellationToken cancellationToken)
    {
        // Admin role can never be self-assigned
        if (request.Role == UserRole.Admin)
            throw new DomainException("Admin role cannot be self-assigned.");

        // Invalidate previous unused OTPs
        var previousOtps = await db.OtpCodes
            .Where(o => o.PhoneNumber == request.PhoneNumber && !o.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (var otp in previousOtps)
            otp.MarkAsUsed();

        // Generate and save new OTP
        var code = Random.Shared.Next(100_000, 999_999).ToString();
        var otpCode = new OtpCode(request.PhoneNumber, code, DateTime.UtcNow.AddMinutes(2));

        await db.OtpCodes.AddAsync(otpCode, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        // Send SMS
        await smsService.SendOtpAsync(request.PhoneNumber, code);

        return true;
    }
}
