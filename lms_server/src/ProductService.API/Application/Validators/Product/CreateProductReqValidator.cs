using FluentValidation;
using ProductService.API.Application.DTOs.Product;

namespace ProductService.API.Application.Validators.Product;
public class CreateProductReqValidator : AbstractValidator<CreateProductReq>
{
    public CreateProductReqValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .NotNull()
            .WithMessage("Name is required")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters long");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .NotNull()
            .WithMessage("Description is required")
            .MinimumLength(10)
            .WithMessage("Description must be at least 10 characters long");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("Price is required")
            .NotNull()
            .WithMessage("Price is required")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");

        RuleFor(x => x.Stock)
            .NotEmpty()
            .WithMessage("Stock is required")
            .NotNull()
            .WithMessage("Stock is required")
            .GreaterThan(0)
            .WithMessage("Stock must be greater than 0");

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .WithMessage("ImageUrl is required")
            .NotNull()
            .WithMessage("ImageUrl is required");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required")
            .NotNull()
            .WithMessage("CategoryId is required");
    }   
}