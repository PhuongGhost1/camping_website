using FluentValidation;
using OrderService.API.Application.DTOs.OrderItem;

namespace OrderService.API.Application.Validators.Order;
public class UpdateOrderItemRequestValidator : AbstractValidator<UpdateOrderItemRequest>
{
    public UpdateOrderItemRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID is required")
            .NotNull()
            .WithMessage("Order ID is required");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required")
            .NotNull()
            .WithMessage("Quantity is required")
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required")
            .NotNull()
            .WithMessage("Product ID is required");
    }
}