using MushroomB2B.Domain.Common;
using MushroomB2B.Domain.ValueObjects;
using MushroomB2B.Domain.Exceptions;

namespace MushroomB2B.Domain.Entities;

public sealed class Shop : BaseEntity
{
    public string OwnerName { get; private set; }
    public Address Address { get; private set; } = default!;
    public decimal CreditLimit { get; private set; }
    public decimal WalletBalance { get; private set; }
    public bool IsVerified { get; private set; } = false;

    private readonly List<Order> _orders = [];
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();
    public Guid? UserId { get; private set; }

    // Required by EF Core
    private Shop() { OwnerName = string.Empty; }

    public Shop(string ownerName, Address address, decimal creditLimit)
    {
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("Owner name is required.", nameof(ownerName));
        if (creditLimit < 0)
            throw new ArgumentOutOfRangeException(nameof(creditLimit), "Credit limit cannot be negative.");

        OwnerName = ownerName;
        Address = address ?? throw new ArgumentNullException(nameof(address));
        CreditLimit = creditLimit;
        WalletBalance = 0m;
    }

    /// <summary>
    /// Checks whether the shop has sufficient balance or credit to place an order.
    /// </summary>
    public void PlaceOrder(decimal orderAmount)
    {
        var available = WalletBalance + CreditLimit;
        if (orderAmount > available)
            throw new DomainException(
                $"Insufficient funds. Required: {orderAmount:C}, Available: {available:C}");
    }

    /// <summary>
    /// Deducts the order amount from wallet. Remaining amount rolls into credit usage.
    /// </summary>
    public void DeductForOrder(decimal amount)
    {
        PlaceOrder(amount); // guard check
        if (WalletBalance >= amount)
        {
            WalletBalance -= amount;
        }
        else
        {
            var remaining = amount - WalletBalance;
            WalletBalance = 0;
            CreditLimit -= remaining;
        }
        SetModified();
    }

    public void ChargeWallet(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Charge amount must be positive.");
        WalletBalance += amount;
        SetModified();
    }

    public void UpdateAddress(Address newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        SetModified();
    }

    public void Verify()
    {
        IsVerified = true;
        SetModified();
    }

    public void AssignUser(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        UserId = userId;
        SetModified();
    }
}
