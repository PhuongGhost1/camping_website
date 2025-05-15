namespace OrderService.API.Application.DTOs.Order;

public record PaymentCompletedEventBus
{
    public Guid OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Timestamp { get; set; }
}