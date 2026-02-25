using FluentValidation;
using MushroomB2B.Domain.Enums;

namespace MushroomB2B.Application.Features.Auth.Commands.RequestOtp;

public sealed class RequestOtpValidator : AbstractValidator<RequestOtpCommand>
{
    public RequestOtpValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^09[0-9]{9}$")
            .WithMessage("PhoneNumber must be a valid Iranian mobile number.");

        RuleFor(x => x.Role)
            .Must(r => r != UserRole.Admin)
            .WithMessage("Admin role cannot be requested.")
            .When(x => x.Role.HasValue);
    }
}
