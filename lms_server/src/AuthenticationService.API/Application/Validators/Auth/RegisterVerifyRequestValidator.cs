using AuthenticationService.API.Application.DTOs.Authentication;
using FluentValidation;

namespace AuthenticationService.API.Application.Validators.Auth;
public class RegisterVerifyRequestValidator : AbstractValidator<RegisterVerifyRequest>
{
    public RegisterVerifyRequestValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.name)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters");

        RuleFor(x => x.pwd)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(5)
            .WithMessage("Password must be at least 5 characters long")
            .MaximumLength(50)
            .WithMessage("Password must not exceed 50 characters");
    }
}