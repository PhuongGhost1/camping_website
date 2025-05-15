namespace PaymentService.API.Application.DTOs.Payment;

public record ProcessCreatePaymentReq
{
    public required decimal Total { get; set; }
    public required string ReturnUrl { get; set; }
    public required string CancelUrl { get; set; }
}

public record ProcessPaymentReq
{
    public required Guid OrderId { get; set; }
    public required decimal Total { get; set; }
}

public record ConfirmPaymentReq
{
    public required string PaymentId { get; set; }
    public required string Token { get; set; }
    public required string PayerId { get; set; }
}
