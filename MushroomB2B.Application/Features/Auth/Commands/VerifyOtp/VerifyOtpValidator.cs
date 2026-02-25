using FluentValidation;

namespace MushroomB2B.Application.Features.Auth.Commands.VerifyOtp;

public sealed class VerifyOtpValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^09[0-9]{9}$").WithMessage("PhoneNumber must be a valid Iranian mobile number.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .Length(6).WithMessage("Code must be exactly 6 characters.")
            .Matches(@"^\d{6}$").WithMessage("Code must contain digits only.");
    }
}
