using FluentValidation;
using PaymentService.Grpc;

namespace PaymentService.API.Application.Validators.Payment;
public class GetAllPaymentReqValidator : AbstractValidator<GetAllPaymentGrpcRequest>
{
    public GetAllPaymentReqValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.")
            .Must(BeAValidGuid).WithMessage("OrderId must be a valid GUID.");
    }

    private bool BeAValidGuid(string orderId)
    {
        return Guid.TryParse(orderId, out _);
    }
}