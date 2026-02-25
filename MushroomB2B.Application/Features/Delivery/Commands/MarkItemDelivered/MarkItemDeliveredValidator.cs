using FluentValidation;

namespace MushroomB2B.Application.Features.Delivery.Commands.MarkItemDelivered;

public sealed class MarkItemDeliveredValidator : AbstractValidator<MarkItemDeliveredCommand>
{
    public MarkItemDeliveredValidator()
    {
        RuleFor(x => x.DeliveryItemId)
            .NotEmpty().WithMessage("DeliveryItemId is required.");

        RuleFor(x => x.DriverId)
            .NotEmpty().WithMessage("DriverId is required.");
    }
}
