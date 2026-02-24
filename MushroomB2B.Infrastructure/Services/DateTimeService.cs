using MushroomB2B.Application.Interfaces;

namespace MushroomB2B.Infrastructure.Services;

public sealed class DateTimeService : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
