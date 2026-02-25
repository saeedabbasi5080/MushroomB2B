using MediatR;

namespace MushroomB2B.Application.Features.Admin.Commands.VerifyShop;

public sealed record VerifyShopCommand(Guid ShopId) : IRequest<VerifyShopResult>;

public sealed record VerifyShopResult(Guid ShopId, bool IsVerified);
