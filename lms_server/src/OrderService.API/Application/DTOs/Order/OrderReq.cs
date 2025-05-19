namespace OrderService.API.Application.DTOs.Order;

public record CreateOrderReq(
    Guid UserId,
    decimal TotalAmount
);

public record UpdateTotalOrderReq(
    decimal TotalAmount
);

public record PublishOrderReq(
    Guid OrderId,
    decimal Total
);