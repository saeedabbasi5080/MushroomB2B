using MediatR;

namespace MushroomB2B.Application.Features.Shops.Commands.ChargeWallet;

public sealed record ChargeWalletCommand : IRequest<ChargeWalletResult>
{
    public required Guid ShopId { get; init; }
    public required decimal Amount { get; init; }
}

public sealed record ChargeWalletResult(Guid ShopId, decimal NewWalletBalance);
