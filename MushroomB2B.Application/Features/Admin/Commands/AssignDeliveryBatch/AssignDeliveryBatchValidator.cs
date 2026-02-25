using FluentValidation;

namespace MushroomB2B.Application.Features.Admin.Commands.AssignDeliveryBatch;

public sealed class AssignDeliveryBatchValidator : AbstractValidator<AssignDeliveryBatchCommand>
{
    public AssignDeliveryBatchValidator()
    {
        RuleFor(x => x.DriverId)
            .NotEmpty().WithMessage("DriverId is required.");

        RuleFor(x => x.OrderIds)
            .NotEmpty().WithMessage("At least one OrderId is required.")
            .Must(ids => ids.Count <= 100).WithMessage("Cannot assign more than 100 orders per batch.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Duplicate OrderIds are not allowed.");

        RuleFor(x => x.BatchDate)
            .NotEmpty().WithMessage("BatchDate is required.")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("BatchDate cannot be in the past.");
    }
}
