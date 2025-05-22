using FluentValidation;
using UserService.API.Application.DTOs.User;

namespace UserService.API.Application.Validators.User;
public class UpdateUserPwdReqValidator : AbstractValidator<UpdateUserPwdReq>
{
    public UpdateUserPwdReqValidator()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .MinimumLength(5)
            .WithMessage("New password must be at least 5 characters long.")
            .MaximumLength(50)
            .WithMessage("New password must not exceed 50 characters.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required.")
            .MinimumLength(5)
            .WithMessage("Confirm password must be at least 5 characters long.")
            .MaximumLength(50)
            .WithMessage("Confirm password must not exceed 50 characters.")
            .Equal(x => x.NewPassword)
            .WithMessage("Confirm password must match the new password.");
    }
}