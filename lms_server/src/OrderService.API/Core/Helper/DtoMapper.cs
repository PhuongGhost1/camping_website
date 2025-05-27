using OrderService.API.Application.DTOs.Order;
using OrderService.API.Application.DTOs.OrderItem;
using OrderService.API.Domain;

namespace OrderService.API.Core.Helper;

public static class DtoMapper
{
    public static OrderItemDto ToMapperDto(this Orderitems orderitems)
    => new OrderItemDto
    {
        Id = orderitems.Id,
        ProductId = orderitems.ProductId,
        Quantity = orderitems.Quantity,
        Price = orderitems.Price,
        Status = orderitems.Status
    };
    
    public static OrderDto ToMapperDto(this Orders orders)
    => new OrderDto
    {
        Id = orders.Id,
        UserId = orders.UserId,
        TotalAmount = orders.TotalAmount,
        Status = orders.Status,
        OrderItems = orders.Orderitems.Select(x => x.ToMapperDto()).ToList()
    };
}