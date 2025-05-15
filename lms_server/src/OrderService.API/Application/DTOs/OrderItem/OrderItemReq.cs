namespace OrderService.API.Application.DTOs.OrderItem;

public record AddToCartRequest
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public record UpdateOrderItemRequest
{
    public Guid ProductId { get; set; }
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal ActualPrice { get; set; }
}

public record DeleteOrderItemRequest
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
}