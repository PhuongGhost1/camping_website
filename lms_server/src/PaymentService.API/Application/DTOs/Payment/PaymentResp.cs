namespace PaymentService.API.Application.DTOs.Payment;

public record ProcessPaymentResp
{
    public string ApprovalUrl { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
}