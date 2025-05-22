using OrderService.API.Application.Bindings;
using OrderService.API.Application.DTOs.Order;
using OrderService.API.Application.DTOs.OrderItem;
using OrderService.API.Application.Services;
using OrderService.API.Application.Validators;

namespace OrderService.API.Application.Endpoints;
public static class OrderEndpoints
{
    public static RouteGroupBuilder MapOrderEndpoints(this RouteGroupBuilder group)
    {
        var orderGroup = group.WithTags("Order");

        orderGroup.MapGet("/order", async (IOrderServices orderService, UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var orders = await orderService.GetOrderByUserId(userId);
            return Results.Ok(orders);
        })
        .WithName("GetOrderByUserId")
        .RequireAuthorization();

        orderGroup.MapGet("/all-order-products", async (IOrderItemServices orderItemService, Guid orderId) =>
        {
            var orderProducts = await orderItemService.GetOrderProducts(orderId);
            return Results.Ok(orderProducts);
        })
        .WithName("GetOrderProducts")
        .RequireAuthorization();

        orderGroup.MapPost("/add-to-cart", async (IOrderItemServices orderItemService, AddToCartRequest req) =>
        {
            var result = await orderItemService.AddOrderItem(req);
            return Results.Ok(result);
        })
        .WithName("AddOrderItem")
        .RequireAuthorization();

        orderGroup.MapPut("/update-order-item", async (IOrderItemServices orderItemService, UpdateOrderItemRequest req) =>
        {
            var result = await orderItemService.UpdateOrderItem(req);
            return Results.Ok(result);
        })
        .WithName("UpdateOrderItem")
        .RequireAuthorization()
        .WithValidation<UpdateOrderItemRequest>();

        orderGroup.MapPost("/delete-order-item", async (IOrderItemServices orderItemService, DeleteOrderItemRequest req) =>
        {
            var result = await orderItemService.DeleteOrderItem(req);
            return Results.Ok(result);
        })
        .WithName("DeleteOrderItem")
        .RequireAuthorization()
        .WithValidation<DeleteOrderItemRequest>();

        orderGroup.MapPut("/update-order-total-amount", async (IOrderServices orderService, UpdateTotalOrderReq req,
        UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var result = await orderService.UpdateOrderTotalAmount(userId, Math.Round(req.TotalAmount, 2));
            return Results.Ok(result);
        })
        .WithName("UpdateOrderTotalAmount")
        .RequireAuthorization();

        orderGroup.MapGet("/all-orders", async (IOrderServices orderService, UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var orders = await orderService.GetAllOrdersByUserId(userId);
            return Results.Ok(orders);
        })
        .WithName("GetAllOrdersByUserId")
        .RequireAuthorization();

        return orderGroup;
    }
}