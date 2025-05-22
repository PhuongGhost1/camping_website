using PaymentService.API.Application.DTOs.Payment;
using PaymentService.API.Application.Services;
using PaymentService.API.Application.Validators;

namespace PaymentService.API.Application.Endpoints;
public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoints(this RouteGroupBuilder group)
    {
        var paymentGroup = group.WithTags("Payment");

        paymentGroup.MapPost("/process-payment", async (IPaymentServices paymentServices, ProcessPaymentReq req) =>
        {
            var result = await paymentServices.ProcessPaymentWithPaypal(req);
            return Results.Ok(result);
        })
        .WithName("ProcessPayment")
        .RequireAuthorization()
        .WithValidation<ProcessPaymentReq>();

        paymentGroup.MapGet("/confirm-payment", async (IPaymentServices paymentServices, [AsParameters] ConfirmPaymentReq req) =>
        {
            var result = await paymentServices.ConfirmPayment(req);
            return Results.Ok(result);
        })
        .WithName("ConfirmPayment")
        .RequireAuthorization()
        .WithValidation<ConfirmPaymentReq>();

        paymentGroup.MapGet("/all-payments", async (IPaymentServices paymentServices, Guid orderId) =>
        {
            var result = await paymentServices.GetAllPaymentByOrderId(orderId);
            return Results.Ok(result);
        })
        .WithName("GetAllPaymentByOrderId")
        .RequireAuthorization();

        paymentGroup.MapGet("/stastic-payment-in-year", async (IPaymentServices paymentServices, int year) =>
        {
            var result = await paymentServices.StasticPaymentInYear(year);
            return Results.Ok(result);
        })
        .WithName("StasticPaymentInYear");

        return paymentGroup;
    }
}