using System.Data;
using FluentValidation;
using ProductService.API.Application.DTOs.Product;

namespace ProductService.API.Application.Validators.Product;
public class UpdateProductReqValidator : AbstractValidator<UpdateProductReq>
{
    public UpdateProductReqValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required")
            .NotNull()
            .WithMessage("Id is required");
    }
}