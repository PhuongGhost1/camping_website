using FluentValidation;
using UserService.API.Application.DTOs.User;

namespace UserService.API.Application.Validators.User;
public class UpdateUserInfoReqValidator : AbstractValidator<UpdateUserInfoReq>
{
    public UpdateUserInfoReqValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.");
    }
}