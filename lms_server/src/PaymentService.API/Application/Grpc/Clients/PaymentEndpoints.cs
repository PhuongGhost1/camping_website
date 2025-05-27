using PaymentService.API.Application.Validators;
using PaymentService.Grpc;

namespace PaymentService.API.Application.Grpc.Clients;
public static class PaymentEndpoints
{
    public static RouteGroupBuilder MapPaymentEndpoints(this RouteGroupBuilder group)
    {
        var paymentGroup = group.WithTags("Payment");

        paymentGroup.MapPost("/process-payment", async (PaymentGrpcClient _client, ProcessPaymentGrpcRequest req) =>
        {
            var result = await _client.ProcessPaymentAsync(req);
            return Results.Ok(result);
        })
        .WithName("ProcessPayment")
        .RequireAuthorization()
        .WithValidation<ProcessPaymentGrpcRequest>();

        paymentGroup.MapGet("/confirm-payment", async (PaymentGrpcClient _client, [AsParameters] ConfirmPaymentGrpcRequest req) =>
        {
            var result = await _client.ConfirmPaymentAsync(req);
            return Results.Ok(result);
        })
        .WithName("ConfirmPayment")
        .RequireAuthorization()
        .WithValidation<ConfirmPaymentGrpcRequest>();

        paymentGroup.MapGet("/all-payments", async (PaymentGrpcClient _client, [AsParameters] GetAllPaymentGrpcRequest req) =>
        {
            var result = await _client.GetAllPaymentByOrderIdAsync(req);
            return Results.Ok(result);
        })
        .WithName("GetAllPaymentByOrderId")
        .RequireAuthorization()
        .WithValidation<GetAllPaymentGrpcRequest>();

        paymentGroup.MapGet("/stastic-payment-in-year", async (PaymentGrpcClient _client, [AsParameters] StasticPaymentGrpcRequest req) =>
        {
            var result = await _client.StasticPaymentInYearAsync(req);
            return Results.Ok(result);
        })
        .WithName("StasticPaymentInYear")
        .RequireAuthorization()
        .WithValidation<StasticPaymentGrpcRequest>();

        return paymentGroup;
    }
}