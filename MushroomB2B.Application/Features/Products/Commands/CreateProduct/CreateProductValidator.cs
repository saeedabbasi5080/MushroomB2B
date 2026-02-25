using FluentValidation;

namespace MushroomB2B.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200);

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("BasePrice must be greater than 0.");
    }
}
