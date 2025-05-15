namespace OrderService.API.Application.DTOs.Order;

public record CreateOrderReq(
    Guid UserId,
    decimal TotalAmount
);

public record UpdateTotalOrderReq(
    decimal TotalAmount
);