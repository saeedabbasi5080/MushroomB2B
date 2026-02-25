namespace MushroomB2B.Application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? PhoneNumber { get; }
    string? Role { get; }
    bool IsAuthenticated { get; }
}
