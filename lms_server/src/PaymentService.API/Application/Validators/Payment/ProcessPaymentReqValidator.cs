using FluentValidation;
using PaymentService.API.Application.DTOs.Payment;
using PaymentService.Grpc;

namespace PaymentService.API.Application.Validators.Payment;

public class ProcessPaymentReqValidator : AbstractValidator<ProcessPaymentGrpcRequest>
{
    public ProcessPaymentReqValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID is required")
            .NotNull()
            .WithMessage("Order ID is required");

        RuleFor(x => x.Total)
            .NotEmpty()
            .WithMessage("Total amount is required")
            .NotNull()
            .WithMessage("Total amount is required")
            .GreaterThan(0)
            .WithMessage("Total amount must be greater than zero");
    }
}