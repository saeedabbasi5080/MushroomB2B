using FluentValidation;

namespace MushroomB2B.Application.Features.Admin.Commands.VerifyShop;

public sealed class VerifyShopValidator : AbstractValidator<VerifyShopCommand>
{
    public VerifyShopValidator()
    {
        RuleFor(x => x.ShopId)
            .NotEmpty().WithMessage("ShopId is required.");
    }
}
