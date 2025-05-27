using PaymentService.Grpc;

namespace PaymentService.API.Application.Grpc.Clients;

public class PaymentGrpcClient
{
    private readonly PaymentServiceGrpc.PaymentServiceGrpcClient _client;
    public PaymentGrpcClient(PaymentServiceGrpc.PaymentServiceGrpcClient client)
    {
        _client = client;
    }

    public async Task<ProcessPaymentGrpcResponse> ProcessPaymentAsync(ProcessPaymentGrpcRequest request)
    {
        return await _client.ProcessPaymentAsync(request);
    }

    public async Task<GenericMessageGrpcResponse> ConfirmPaymentAsync(ConfirmPaymentGrpcRequest request)
    {
        return await _client.ConfirmPaymentAsync(request);
    }

    public async Task<ListPaymentsGrpcResponse> GetAllPaymentByOrderIdAsync(GetAllPaymentGrpcRequest request)
    {
        return await _client.GetAllPaymentByOrderIdAsync(request);
    }
    
    public async Task<StasticPaymentGrpcResponse> StasticPaymentInYearAsync(StasticPaymentGrpcRequest request)
    {
        return await _client.StasticPaymentInYearAsync(request);
    }
}