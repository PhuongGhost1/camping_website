using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.Grpc;
using FluentValidation;

namespace AuthenticationService.API.Application.Validators.Auth;
public class RegisterVerifyRequestValidator : AbstractValidator<RegisterVerifyGrpcRequest>
{
    public RegisterVerifyRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Name must be at least 6 characters long")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(5)
            .WithMessage("Password must be at least 5 characters long")
            .MaximumLength(50)
            .WithMessage("Password must not exceed 50 characters");
    }
}