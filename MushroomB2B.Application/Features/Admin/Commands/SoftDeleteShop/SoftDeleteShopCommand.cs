using MediatR;

namespace MushroomB2B.Application.Features.Admin.Commands.SoftDeleteShop;

public sealed record SoftDeleteShopCommand(Guid ShopId) : IRequest<bool>;
