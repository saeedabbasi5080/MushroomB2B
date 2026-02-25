using FluentValidation;

namespace MushroomB2B.Application.Features.Delivery.Commands.MarkItemFailed;

public sealed class MarkItemFailedValidator : AbstractValidator<MarkItemFailedCommand>
{
    public MarkItemFailedValidator()
    {
        RuleFor(x => x.DeliveryItemId)
            .NotEmpty().WithMessage("DeliveryItemId is required.");

        RuleFor(x => x.DriverId)
            .NotEmpty().WithMessage("DriverId is required.");

        RuleFor(x => x.FailureReason)
            .NotEmpty().WithMessage("FailureReason is required.")
            .MaximumLength(500).WithMessage("FailureReason cannot exceed 500 characters.");
    }
}
