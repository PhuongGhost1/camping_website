using FluentValidation;
using OrderService.API.Application.DTOs.Order;

namespace OrderService.API.Application.Validators.Order;
public class UpdateTotalOrderReqValidator : AbstractValidator<UpdateTotalOrderReq>
{
    public UpdateTotalOrderReqValidator()
    {
        RuleFor(x => x.TotalAmount)
            .NotEmpty()
            .WithMessage("Total amount is required")
            .NotNull()
            .WithMessage("Total amount is required")
            .GreaterThan(0)
            .WithMessage("Total amount must be greater than 0");
    }
}