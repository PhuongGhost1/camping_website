using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Application.DTOs.Payment;
using PaymentService.API.Application.Grpc.Extensions;
using PaymentService.API.Application.Services;
using PaymentService.API.Domain;
using PaymentService.Grpc;
using StasticInMonth = PaymentService.API.Application.DTOs.Payment.StatisticMonth;

namespace PaymentService.API.Application.Grpc.Services;

public class PaymentGrpcService : PaymentServiceGrpc.PaymentServiceGrpcBase
{
    private readonly IPaymentServices _paymentServices;
    public PaymentGrpcService(IPaymentServices paymentServices)
    {
        _paymentServices = paymentServices;
    }

    public override async Task<ProcessPaymentGrpcResponse> ProcessPayment(ProcessPaymentGrpcRequest request, ServerCallContext context)
    {
        var result = await _paymentServices.ProcessPaymentWithPaypal(request.ToProcessPaymentReq()) as JsonResult;
        var message = result?.Value as ProcessPaymentResp;

        return message?.ToProcessPaymentResp() ?? new ProcessPaymentGrpcResponse();
    }

    public override async Task<GenericMessageGrpcResponse> ConfirmPayment(ConfirmPaymentGrpcRequest request, ServerCallContext context)
    {
        var result = await _paymentServices.ConfirmPayment(request.ToConfirmPaymentReq()) as JsonResult;
        var message = PaymentGrpcMapper.ExtractMessageFromResult(result);

        return PaymentGrpcMapper.ToGenericMessageGrpcResponse(message ?? "Payment confirmation failed!");
    }

    public override async Task<ListPaymentsGrpcResponse> GetAllPaymentByOrderId(GetAllPaymentGrpcRequest request, ServerCallContext context)
    {
        var result = await _paymentServices.GetAllPaymentByOrderId(Guid.Parse(request.OrderId)) as JsonResult;
        var message = result?.Value as IEnumerable<Payments>;

        return message?.ToPaymentsGrpcResponse() ?? new ListPaymentsGrpcResponse();
    }

    public override async Task<StasticPaymentGrpcResponse> StasticPaymentInYear(StasticPaymentGrpcRequest request, ServerCallContext context)
    {
        var result = await _paymentServices.StasticPaymentInYear(request.Year) as JsonResult;
        var message = result?.Value as IEnumerable<StasticInMonth>;

        return message?.ToStasticPaymentGrpcResponse() ?? new StasticPaymentGrpcResponse();
    }
}