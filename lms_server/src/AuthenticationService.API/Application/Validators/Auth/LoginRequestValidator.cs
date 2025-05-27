using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.Grpc;
using FluentValidation;

namespace AuthenticationService.API.Application.Validators.Auth;
public class LoginRequestValidator : AbstractValidator<LoginGrpcRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .NotNull()
            .WithMessage("Email is required")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters")
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .NotNull()
            .WithMessage("Password is required");
    }
}