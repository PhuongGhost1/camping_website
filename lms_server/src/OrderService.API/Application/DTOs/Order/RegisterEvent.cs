namespace OrderService.API.Application.DTOs.Order;
public class RegisterEvent
{
    public Guid UserId { get; set; }
    public DateTime Timestamp { get; set; }
}