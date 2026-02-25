using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User?.FindFirstValue("sub");

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string? PhoneNumber =>
        User?.FindFirstValue("phone");

    public string? Role =>
        User?.FindFirstValue(ClaimTypes.Role)
        ?? User?.FindFirstValue("role");
}
