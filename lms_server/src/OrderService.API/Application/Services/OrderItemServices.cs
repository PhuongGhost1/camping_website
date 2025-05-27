using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.DTOs.OrderItem;
using OrderService.API.Application.Shared.Enum;
using OrderService.API.Application.Shared.Type;
using OrderService.API.Core.Helper;
using OrderService.API.Domain;
using OrderService.API.Infrastructure.Repository;

namespace OrderService.API.Application.Services;

public interface IOrderItemServices
{
    Task<IActionResult> GetOrderProducts(Guid orderId);
    Task<IActionResult> AddOrderItem(AddToCartRequest request);
    Task<IActionResult> UpdateOrderItem(UpdateOrderItemRequest request);
    Task<IActionResult> DeleteOrderItem(DeleteOrderItemRequest request);
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

            var orderItem = await _orderItemRepo.GetOrderItemById(request.ProductId, request.OrderId);
            if (orderItem != null)
            {
                orderItem.Quantity = orderItem.Quantity + request.Quantity;
                orderItem.Price = orderItem.Quantity * request.Price;

                var isUpdated = await _orderItemRepo.UpdateOrderItem(orderItem);
                if (!isUpdated) return ErrorResp.BadRequest("Failed to update order item!");

                order.TotalAmount = order.TotalAmount + (request.Quantity * request.Price);
                await _orderRepo.UpdateOrder(order);

                return SuccessResp.Ok(orderItem.ToMapperDto());
            }    

            var orderItemObj = new Orderitems
            {
                OrderId = request.OrderId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Status = OrderStatusEnum.Processing.ToString(),
                Price = request.Quantity * request.Price
            };

            order.TotalAmount = order.TotalAmount + orderItemObj.Price;

            await _orderRepo.UpdateOrder(order);

            var isCreated = await _orderItemRepo.AddOrderItem(orderItemObj);
            if (!isCreated) return ErrorResp.BadRequest("Failed to create order item!");

            return SuccessResp.Created(orderItemObj.ToMapperDto());
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> DeleteOrderItem(DeleteOrderItemRequest request)
    {
        try
        {
            var orderItem = await _orderItemRepo.GetOrderItemById(request.ProductId, request.OrderId);
            if (orderItem == null)
                return ErrorResp.NotFound("Order item not found!");

            var isDeleted = await _orderItemRepo.DeleteOrderItem(orderItem.Id);
            if (isDeleted == 0) return ErrorResp.BadRequest("Failed to delete order item!");

            return SuccessResp.Ok("Order item deleted successfully!");
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
                return SuccessResp.Ok(new List<Orderitems>());

            return SuccessResp.Ok(orderItems);    
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> UpdateOrderItem(UpdateOrderItemRequest request)
    {
        try
        {
            var orderItem = await _orderItemRepo.GetOrderItemById(request.ProductId, request.OrderId);
            if (orderItem == null)
                return ErrorResp.NotFound("Order item not found!");

            if (request.Quantity < 0)
                return ErrorResp.BadRequest("Quantity must be greater than 0!");

            orderItem.Quantity = request.Quantity;
            orderItem.Price = request.Quantity * request.ActualPrice;

            if(orderItem.Quantity == 0)
            {
                var isDeleted = await _orderItemRepo.DeleteOrderItem(orderItem.Id);
                if (isDeleted == 0) return ErrorResp.BadRequest("Failed to delete order item!");

                return SuccessResp.Ok("Order item deleted successfully!");
            }

            var isUpdated = await _orderItemRepo.UpdateOrderItem(orderItem);
            if (!isUpdated) return ErrorResp.BadRequest("Failed to update order item!");

            return SuccessResp.Ok(orderItem);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}