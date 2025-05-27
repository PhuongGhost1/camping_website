using FluentValidation;
using PaymentService.Grpc;

namespace PaymentService.API.Application.Validators.Payment;
public class ConfirmPaymentReqValidator : AbstractValidator<ConfirmPaymentGrpcRequest>
{
    public ConfirmPaymentReqValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("PaymentId is required.");

        RuleFor(x => x.PayerId)
            .NotEmpty()
            .WithMessage("PayerId is required.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required.");
    }
}