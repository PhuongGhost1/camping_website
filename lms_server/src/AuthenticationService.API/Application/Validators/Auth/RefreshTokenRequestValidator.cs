using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.Grpc;
using FluentValidation;

namespace AuthenticationService.API.Application.Validators.Auth;
public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenGrpcRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required")
            .NotNull()
            .WithMessage("Refresh token is required")
            .MaximumLength(100)
            .WithMessage("Refresh token must not exceed 100 characters");
    }
}