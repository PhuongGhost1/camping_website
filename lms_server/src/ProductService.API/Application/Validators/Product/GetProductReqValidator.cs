using FluentValidation;
using ProductService.API.Application.DTOs.Product;

namespace ProductService.API.Application.Validators.Product;
public class GetProductReqValidator : AbstractValidator<GetProductReq>
{
    public GetProductReqValidator()
    {
        RuleFor(x => x.Page)
            .NotEmpty()
            .WithMessage("Page is required")
            .NotNull()
            .WithMessage("Page is required")
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .NotEmpty()
            .WithMessage("PageSize is required")
            .NotNull()
            .WithMessage("PageSize is required")
            .GreaterThan(0)
            .WithMessage("PageSize must be greater than 0");        
    }
}