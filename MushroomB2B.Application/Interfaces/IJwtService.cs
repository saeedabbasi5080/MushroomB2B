using System.Security.Claims;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string phone, UserRole role);
    ClaimsPrincipal? ValidateToken(string token);
}
