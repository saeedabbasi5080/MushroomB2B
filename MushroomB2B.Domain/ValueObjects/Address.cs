namespace MushroomB2B.Domain.ValueObjects;

public sealed record Address
{
    public required string City { get; init; }
    public required string Street { get; init; }
    public double? GeoLat { get; init; }
    public double? GeoLng { get; init; }

    // EF Core requires a parameterless constructor for owned types
    //private Address() { City = string.Empty; Street = string.Empty; }
    public Address() { City = string.Empty; Street = string.Empty; }

    public Address(string city, string street, double? geoLat = null, double? geoLng = null)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty.", nameof(city));
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException("Street cannot be empty.", nameof(street));

        City = city;
        Street = street;
        GeoLat = geoLat;
        GeoLng = geoLng;
    }

    public override string ToString() => $"{Street}, {City}";
}
