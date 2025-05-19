using OrderService.API.Domain;

namespace OrderService.API.Infrastructure.Repository;

public interface IOrderRepository
{
    Task<bool> CreateOrder(Orders order);
    Task<Orders?> GetOrderByUserId(Guid? userId);
    Task<bool> CheckIfExistProcessingOrder(Guid? userId);
    Task<Orders?> GetOrderById(Guid? orderId);
    Task<bool> UpdateOrder(Orders order);
    Task<IEnumerable<Orders>> GetAllOrdersByUserId(Guid? userId);
}