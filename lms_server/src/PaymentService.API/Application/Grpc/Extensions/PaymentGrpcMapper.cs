using System.Security.Claims;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Application.DTOs.Payment;
using PaymentService.API.Domain;
using PaymentService.Grpc;
using ProtoPayment = PaymentService.Grpc.Payment;
using ProtoStatisticMonth = PaymentService.Grpc.StatisticMonth;
using StasticInMonth = PaymentService.API.Application.DTOs.Payment.StatisticMonth;

namespace PaymentService.API.Application.Grpc.Extensions;

public static class PaymentGrpcMapper
{
    public static ProcessPaymentReq ToProcessPaymentReq(this ProcessPaymentGrpcRequest req)
    => new ProcessPaymentReq
    {
        OrderId = Guid.Parse(req.OrderId),
        Total = (decimal)req.Total
    };

    public static ProcessPaymentGrpcResponse ToProcessPaymentResp(this ProcessPaymentResp resp)
    => new ProcessPaymentGrpcResponse
    {
        ApprovalUrl = resp.ApprovalUrl,
        PaymentId = resp.PaymentId
    };

    public static ConfirmPaymentReq ToConfirmPaymentReq(this ConfirmPaymentGrpcRequest req)
    => new ConfirmPaymentReq
    {
        PayerId = req.PayerId,
        PaymentId = req.PaymentId,
        Token = req.Token
    };    

    public static GenericMessageGrpcResponse ToGenericMessageGrpcResponse(this string message)
    => new GenericMessageGrpcResponse
    {
        Message = message
    };

    public static ListPaymentsGrpcResponse ToPaymentsGrpcResponse(this IEnumerable<Payments> payments)
    {
        var response = new ListPaymentsGrpcResponse();
        foreach (var payment in payments)
        {
            response.Payment.Add(new ProtoPayment
            {
                Id = payment.Id.ToString(),
                OrderId = payment.OrderId.ToString(),
                Amount = (double)(payment?.Amount ?? 0),
                PaidAt = payment != null && payment.PaidAt.HasValue ? payment.PaidAt.Value.ToString("o") : string.Empty,
                PaymentMethod = payment?.PaymentMethod?.ToString(),
                Status = payment?.Status?.ToString()
            });
        }
        return response;
    }

    public static StasticPaymentGrpcResponse ToStasticPaymentGrpcResponse(this IEnumerable<StasticInMonth> stats)
    {
        var response = new StasticPaymentGrpcResponse();
        foreach (var stat in stats)
        {
            response.StatisticMonth.Add(new ProtoStatisticMonth
            {
                Month = stat.Month,
                Amount = (double)(stat?.Amount ?? 0)
            });
        }
        return response;
    }

    public static Guid? GetUserIdFromClaims(ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var user = httpContext.User;

        var claim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");
        return Guid.TryParse(claim?.Value, out var userId) ? userId : null;
    }

    public static string? ExtractMessageFromResult(JsonResult? result)
    {
        if (result?.Value is null) return null;

        var prop = result.Value.GetType().GetProperty("Message");
        return prop?.GetValue(result.Value)?.ToString();
    }
}