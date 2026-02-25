using FluentValidation;

namespace MushroomB2B.Application.Features.Shops.Commands.CreateShop;

public sealed class CreateShopValidator : AbstractValidator<CreateShopCommand>
{
    public CreateShopValidator()
    {
        RuleFor(x => x.OwnerName)
            .NotEmpty().WithMessage("OwnerName is required.")
            .MaximumLength(200);

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required.")
            .MaximumLength(100);

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Street is required.")
            .MaximumLength(300);

        RuleFor(x => x.CreditLimit)
            .GreaterThanOrEqualTo(0).WithMessage("CreditLimit cannot be negative.");
    }
}
