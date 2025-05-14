namespace OrderService.API.Application.DTOs.OrderItem;

public record AddToCartRequest
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
