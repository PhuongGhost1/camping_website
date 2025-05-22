using AuthenticationService.API.Application.DTOs.Authentication;
using FluentValidation;

namespace AuthenticationService.API.Application.Validators.Auth;
public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.refreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required")
            .NotNull()
            .WithMessage("Refresh token is required")
            .MaximumLength(100)
            .WithMessage("Refresh token must not exceed 100 characters");
    }
}