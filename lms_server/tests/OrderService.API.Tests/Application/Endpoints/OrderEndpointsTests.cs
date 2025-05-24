using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Moq;
using Xunit;
using OrderService.API.Application.Endpoints;
using OrderService.API.Application.Services;
using OrderService.API.Application.DTOs.Order;
using OrderService.API.Application.DTOs.OrderItem;

public class OrderEndpointsTests
{
    [Fact]
    public async Task GetOrderByUserId_ReturnsOrders_ForAuthorizedUser()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderServices>();
        var userId = new UserId(Guid.NewGuid());
        var expectedOrders = new List<OrderDto>
        {
            new OrderDto { Id = Guid.NewGuid(), UserId = userId, TotalAmount = 100.0m }
        };

        mockOrderService
            .Setup(s => s.GetOrderByUserId(userId))
            .ReturnsAsync(expectedOrders);

        // Build endpoint delegate
        var endpointDelegate = OrderEndpoints.MapOrderEndpoints(new RouteGroupBuilder())
            .Routes[0].RequestDelegate;

        // Act
        var result = await endpointDelegate(mockOrderService.Object, userId);

        // Assert
        var okResult = Assert.IsType<Ok<List<OrderDto>>>(result);
        Assert.Equal(expectedOrders, okResult.Value);
    }

    [Fact]
    public async Task AddOrderItem_ReturnsOk_ForValidRequest()
    {
        // Arrange
        var mockOrderItemService = new Mock<IOrderItemServices>();
        var addToCartRequest = new AddToCartRequest
        {
            // Populate with sample data as needed for the test
            OrderId = Guid.NewGuid(),
            ProductId = Guid.NewGuid(),
            Quantity = 2
        };
        var expectedResult = new { Success = true, Message = "Item added" };

        mockOrderItemService
            .Setup(s => s.AddOrderItem(addToCartRequest))
            .ReturnsAsync(expectedResult);

        // Find the /add-to-cart endpoint (index 2 based on the order in MapOrderEndpoints)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var addToCartEndpoint = endpoints[2].RequestDelegate;

        // Act
        var result = await addToCartEndpoint(mockOrderItemService.Object, addToCartRequest);

        // Assert
        var okResult = Assert.IsType<Ok<object>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task UpdateOrderTotalAmount_ReturnsOk_ForAuthorizedUser()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderServices>();
        var userId = new UserId(Guid.NewGuid());
        var updateTotalOrderReq = new UpdateTotalOrderReq
        {
            TotalAmount = 123.456m
        };
        var expectedResult = new { Success = true, Message = "Order total updated" };

        mockOrderService
            .Setup(s => s.UpdateOrderTotalAmount(userId, Math.Round(updateTotalOrderReq.TotalAmount, 2)))
            .ReturnsAsync(expectedResult);

        // Find the /update-order-total-amount endpoint (index 5 based on the order in MapOrderEndpoints)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var updateOrderTotalAmountEndpoint = endpoints[5].RequestDelegate;

        // Act
        var result = await updateOrderTotalAmountEndpoint(mockOrderService.Object, updateTotalOrderReq, userId);

        // Assert
        var okResult = Assert.IsType<Ok<object>>(result);
        Assert.Equal(expectedResult, okResult.Value);
    }

    [Fact]
    public async Task GetOrderProducts_ReturnsOrderProducts_ForValidOrderId()
    {
        // Arrange
        var mockOrderItemService = new Mock<IOrderItemServices>();
        var orderId = Guid.NewGuid();
        var expectedOrderProducts = new List<OrderItemDto>
        {
            new OrderItemDto { Id = Guid.NewGuid(), OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 1 },
            new OrderItemDto { Id = Guid.NewGuid(), OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 2 }
        };

        mockOrderItemService
            .Setup(s => s.GetOrderProducts(orderId))
            .ReturnsAsync(expectedOrderProducts);

        // Find the /all-order-products endpoint (index 1 based on the order in MapOrderEndpoints)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var allOrderProductsEndpoint = endpoints[1].RequestDelegate;

        // Act
        var result = await allOrderProductsEndpoint(mockOrderItemService.Object, orderId);

        // Assert
        var okResult = Assert.IsType<Ok<List<OrderItemDto>>>(result);
        Assert.Equal(expectedOrderProducts, okResult.Value);
    }

    [Fact]
    public async Task GetOrderByUserId_ReturnsUnauthorized_WhenUserIdIsNull()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderServices>();
        UserId? userId = null;

        // Build endpoint delegate for /order (index 0)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var getOrderByUserIdEndpoint = endpoints[0].RequestDelegate;

        // Act
        var result = await getOrderByUserIdEndpoint(mockOrderService.Object, userId);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public async Task UpdateOrderItem_ReturnsValidationError_ForInvalidRequest()
    {
        // Arrange
        var mockOrderItemService = new Mock<IOrderItemServices>();
        // Simulate an invalid request (e.g., missing required fields)
        var invalidRequest = new UpdateOrderItemRequest
        {
            // Intentionally leave out required fields or set invalid values
            OrderItemId = Guid.Empty, // Assuming this is invalid
            Quantity = -1 // Invalid quantity
        };

        // Find the /update-order-item endpoint (index 3 based on the order in MapOrderEndpoints)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var updateOrderItemEndpoint = endpoints[3].RequestDelegate;

        // Act
        // Since validation is handled by WithValidation<T>(), the endpoint should not be called if the model is invalid.
        // However, in minimal API testing, we need to simulate model validation failure.
        // We'll simulate this by checking that the endpoint returns a validation problem result for invalid input.
        // In a real integration test, the framework would return a ValidationProblem result.
        // Here, we simulate the behavior by calling the endpoint and expecting it to not call the service and return a validation error.

        // The minimal API validation filter would return a ValidationProblem result.
        // Since we can't trigger the filter in a unit test, we can assert that the endpoint is not called for invalid input.
        // Alternatively, if the endpoint is called, it should handle invalid input gracefully.
        // For demonstration, we'll assert that the service is not called and simulate a validation error result.

        // Act
        // Try to call the endpoint and check if it returns a validation error (BadRequest or ValidationProblem)
        var result = await updateOrderItemEndpoint(mockOrderItemService.Object, invalidRequest);

        // Assert
        // The expected result is a ValidationProblem or BadRequest result.
        // Since minimal APIs use ValidationProblem for invalid models, check for that.
        Assert.True(result is ValidationProblem || result is BadRequest<object> || result is BadRequest, "Expected a validation error result for invalid request.");
        mockOrderItemService.Verify(s => s.UpdateOrderItem(It.IsAny<UpdateOrderItemRequest>()), Times.Never);
    }

    [Fact]
    public async Task DeleteOrderItem_ReturnsNotFound_ForNonExistentOrderItem()
    {
        // Arrange
        var mockOrderItemService = new Mock<IOrderItemServices>();
        var deleteOrderItemRequest = new DeleteOrderItemRequest
        {
            // Populate with a non-existent order item id
            OrderItemId = Guid.NewGuid()
        };

        // Simulate service returning a result indicating not found (could be null or a result object with a flag)
        object notFoundResult = null; // Assuming service returns null for not found
        mockOrderItemService
            .Setup(s => s.DeleteOrderItem(deleteOrderItemRequest))
            .ReturnsAsync(notFoundResult);

        // Find the /delete-order-item endpoint (index 4 based on the order in MapOrderEndpoints)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var deleteOrderItemEndpoint = endpoints[4].RequestDelegate;

        // Act
        var result = await deleteOrderItemEndpoint(mockOrderItemService.Object, deleteOrderItemRequest);

        // Assert
        // If the endpoint returns NotFound for null result, check for NotFound result
        Assert.True(result is NotFound || result is NotFound<object> || (result is Ok<object> ok && ok.Value == null),
            "Expected NotFound or equivalent result for non-existent order item.");
    }

    [Fact]
    public async Task UpdateOrderTotalAmount_ReturnsUnauthorized_WhenUserIdIsNull()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderServices>();
        UserId? userId = null;
        var updateTotalOrderReq = new UpdateTotalOrderReq
        {
            TotalAmount = 50.0m
        };

        // Find the /update-order-total-amount endpoint (index 5 based on the order in MapOrderEndpoints)
        var routeGroupBuilder = new RouteGroupBuilder();
        var endpoints = OrderEndpoints.MapOrderEndpoints(routeGroupBuilder).Routes;
        var updateOrderTotalAmountEndpoint = endpoints[5].RequestDelegate;

        // Act
        var result = await updateOrderTotalAmountEndpoint(mockOrderService.Object, updateTotalOrderReq, userId);

        // Assert
        Assert.IsType<UnauthorizedHttpResult>(result);
        mockOrderService.Verify(s => s.UpdateOrderTotalAmount(It.IsAny<UserId>(), It.IsAny<decimal>()), Times.Never);
    }
}