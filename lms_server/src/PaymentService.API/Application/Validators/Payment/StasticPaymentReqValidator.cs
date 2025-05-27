using FluentValidation;
using PaymentService.Grpc;

namespace PaymentService.API.Application.Validators.Payment;
public class StasticPaymentReqValidator : AbstractValidator<StasticPaymentGrpcRequest>
{
    public StasticPaymentReqValidator()
    {
        RuleFor(x => x.Year)
            .NotEmpty()
            .WithMessage("Year is required")
            .NotNull()
            .WithMessage("Year is required")
            .InclusiveBetween(2000, 2100)
            .WithMessage("Year must be between 2000 and 2100");
    }
}