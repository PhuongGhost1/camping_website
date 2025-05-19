using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.DTOs.Order;
using OrderService.API.Application.Shared.Enum;
using OrderService.API.Application.Shared.Type;
using OrderService.API.Domain;
using OrderService.API.Infrastructure.Repository;

namespace OrderService.API.Application.Services;

public interface IOrderServices
{
    Task<IActionResult> CreateOrder(CreateOrderReq order);
    Task<IActionResult> GetOrderByUserId(Guid? userId);
    Task<IActionResult> UpdateOrderTotalAmount(Guid? userId, decimal totalAmount);
    Task<IActionResult> GetAllOrdersByUserId(Guid? userId);
}
public class OrderServices : IOrderServices
{
    private readonly IOrderRepository _orderRepo;
    public OrderServices(IOrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
    }
    public async Task<IActionResult> CreateOrder(CreateOrderReq order)
    {
        try
        {
            var orderObj = new Orders
            {
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                Status = OrderStatusEnum.Processing.ToString(),
                CreatedAt = DateTime.UtcNow
            };

            var isCreated = await _orderRepo.CreateOrder(orderObj);
            if (!isCreated) return ErrorResp.BadRequest("Failed to create order!");

            return SuccessResp.Created("Order created successfully!");
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> GetAllOrdersByUserId(Guid? userId)
    {
        try
        {
            if(userId == null || userId == Guid.Empty)
                return ErrorResp.Unauthorized("User ID is required!");

            var orders = await _orderRepo.GetAllOrdersByUserId(userId);
            if (orders == null)
                return ErrorResp.NotFound("No orders found for this user!");

            return SuccessResp.Ok(orders);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> GetOrderByUserId(Guid? userId)
    {
        try
        {
            if(userId == null || userId == Guid.Empty)
                return ErrorResp.Unauthorized("User ID is required!");

            var order = await _orderRepo.GetOrderByUserId(userId);
            if (order == null)
                return ErrorResp.NotFound("Order not found!");

            return SuccessResp.Ok(order);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> UpdateOrderTotalAmount(Guid? userId, decimal totalAmount)
    {
        try
        {
            var order = await _orderRepo.GetOrderByUserId(userId);
            if (order == null)
                return ErrorResp.NotFound("Order not found!");

            order.TotalAmount = totalAmount;
            var isUpdated = await _orderRepo.UpdateOrder(order);
            if (!isUpdated) return ErrorResp.BadRequest("Failed to update order!");

            return SuccessResp.Ok("Order updated successfully!");    
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}