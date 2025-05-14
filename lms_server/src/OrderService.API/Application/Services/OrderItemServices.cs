using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.DTOs.OrderItem;
using OrderService.API.Application.Shared.Enum;
using OrderService.API.Application.Shared.Type;
using OrderService.API.Domain;
using OrderService.API.Infrastructure.Repository;

namespace OrderService.API.Application.Services;

public interface IOrderItemServices
{
    Task<IActionResult> GetOrderProducts(Guid orderId);
    Task<IActionResult> AddOrderItem(AddToCartRequest request);
}
public class OrderItemServices : IOrderItemServices
{
    private readonly IOrderItemRepository _orderItemRepo;
    private readonly IOrderRepository _orderRepo;
    public OrderItemServices(IOrderItemRepository orderItemRepo, IOrderRepository orderRepo)
    {
        _orderItemRepo = orderItemRepo;
        _orderRepo = orderRepo;
    }
    public async Task<IActionResult> AddOrderItem(AddToCartRequest request)
    {
        try
        {
            var order = await _orderRepo.GetOrderById(request.OrderId);
            if (order == null)
                return ErrorResp.NotFound("Order not found!");

            if (request == null)
                return ErrorResp.BadRequest("Order item is required!");

            var orderItemObj = new Orderitems
            {
                OrderId = request.OrderId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Status = OrderStatusEnum.Processing.ToString(),
                Price = request.Quantity * request.Price
            };

            var isCreated = await _orderItemRepo.AddOrderItem(orderItemObj);
            if (!isCreated) return ErrorResp.BadRequest("Failed to create order item!");

            order.TotalAmount = order.Orderitems.Sum(x => x.Quantity * x.Price);

            var isUpdated = await _orderRepo.UpdateOrder(order);
            if (!isUpdated) return ErrorResp.BadRequest("Failed to update order!");

            return SuccessResp.Created("Order item created successfully!");
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> GetOrderProducts(Guid orderId)
    {
        try
        {
            var orderItems = await _orderItemRepo.GetAllOrderItems(orderId);
            if (orderItems == null || !orderItems.Any())
                return ErrorResp.NotFound("Order items not found!");

            return SuccessResp.Ok(orderItems);    
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}