using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Domain.Entities;

public sealed class User : BaseEntity
{
    public string PhoneNumber { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }

    private User() { }

    public User(string phoneNumber, UserRole role, string fullName = "")
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("PhoneNumber is required.", nameof(phoneNumber));

        PhoneNumber = phoneNumber;
        FullName = fullName;
        Role = role;
    }

    public void SetRefreshToken(string token, DateTime expiresAt)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("RefreshToken cannot be empty.", nameof(token));

        RefreshToken = token;
        RefreshTokenExpiresAt = expiresAt;
        SetModified();
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
        SetModified();
    }

    public void UpdateFullName(string fullName)
    {
        FullName = fullName;
        SetModified();
    }
}
