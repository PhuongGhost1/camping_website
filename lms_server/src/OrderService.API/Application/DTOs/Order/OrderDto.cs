using OrderService.API.Application.DTOs.OrderItem;

namespace OrderService.API.Application.DTOs.Order;
public class OrderDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? Status { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}