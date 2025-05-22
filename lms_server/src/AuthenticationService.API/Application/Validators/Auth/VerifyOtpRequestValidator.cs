using AuthenticationService.API.Application.DTOs.OTP;
using FluentValidation;

namespace AuthenticationService.API.Application.Validators.Auth;
public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpRequest>
{
    public VerifyOtpRequestValidator()
    {
        RuleFor(x => x.Otp)
            .NotEmpty()
            .WithMessage("OTP is required")
            .NotNull()
            .WithMessage("OTP is required")
            .MaximumLength(6)
            .WithMessage("OTP must not exceed 6 characters")
            .MinimumLength(6)
            .WithMessage("OTP must be at least 6 characters long");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .NotNull()
            .WithMessage("Email is required")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters")
            .EmailAddress();
    }
}