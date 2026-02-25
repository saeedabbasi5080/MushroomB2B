using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Domain.Entities;

public sealed class OtpCode : BaseEntity
{
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsUsed { get; private set; }

    private OtpCode() { }

    public OtpCode(string phoneNumber, string code, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("PhoneNumber is required.", nameof(phoneNumber));
        if (string.IsNullOrWhiteSpace(code) || code.Length != 6)
            throw new ArgumentException("Code must be exactly 6 digits.", nameof(code));

        PhoneNumber = phoneNumber;
        Code = code;
        ExpiresAt = expiresAt;
        IsUsed = false;
    }

    public void Verify(string code)
    {
        if (IsUsed)
            throw new DomainException("This OTP has already been used.");
        if (DateTime.UtcNow > ExpiresAt)
            throw new DomainException("OTP has expired.");
        if (Code != code)
            throw new DomainException("Invalid OTP code.");
    }

    public void MarkAsUsed()
    {
        IsUsed = true;
        SetModified();
    }
}
