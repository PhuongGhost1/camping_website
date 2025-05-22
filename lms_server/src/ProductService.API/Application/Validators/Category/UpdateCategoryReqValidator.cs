using FluentValidation;
using ProductService.API.Application.DTOs.Category;

namespace ProductService.API.Application.Validators.Category;
public class UpdateCategoryReqValidator : AbstractValidator<UpdateCategoryReq>
{
    public UpdateCategoryReqValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .Must(x => Guid.TryParse(x.ToString(), out _))
            .WithMessage("Id must be a valid GUID");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(200)
            .WithMessage("Description must not exceed 200 characters");
    }
}