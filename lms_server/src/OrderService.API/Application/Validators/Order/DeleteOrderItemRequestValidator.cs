using FluentValidation;
using OrderService.API.Application.DTOs.OrderItem;

namespace OrderService.API.Application.Validators.Order;
public class DeleteOrderItemRequestValidator : AbstractValidator<DeleteOrderItemRequest>
{
    public DeleteOrderItemRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID is required")
            .NotNull()
            .WithMessage("Order ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required")
            .NotNull()
            .WithMessage("Product ID is required");
    }
}