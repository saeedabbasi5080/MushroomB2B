using MushroomB2B.Domain.Common;

namespace MushroomB2B.Domain.Entities;

public sealed class Driver : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string VehiclePlate { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;

    private Driver() { }

    public Driver(string fullName, string phoneNumber, string vehiclePlate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("FullName is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("PhoneNumber is required.", nameof(phoneNumber));
        if (string.IsNullOrWhiteSpace(vehiclePlate))
            throw new ArgumentException("VehiclePlate is required.", nameof(vehiclePlate));

        FullName = fullName;
        PhoneNumber = phoneNumber;
        VehiclePlate = vehiclePlate;
    }

    public void Deactivate()
    {
        IsActive = false;
        SetModified();
    }

    public void Activate()
    {
        IsActive = true;
        SetModified();
    }
}
