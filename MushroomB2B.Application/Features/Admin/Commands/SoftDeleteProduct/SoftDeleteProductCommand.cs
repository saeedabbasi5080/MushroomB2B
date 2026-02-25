using MediatR;

namespace MushroomB2B.Application.Features.Admin.Commands.SoftDeleteProduct;

public sealed record SoftDeleteProductCommand(Guid ProductId) : IRequest<bool>;
