using OrderService.API.Domain;

namespace OrderService.API.Infrastructure.Repository;
public interface IOrderItemRepository
{
    Task<IEnumerable<Orderitems>> GetAllOrderItems(Guid orderId);
    Task<bool> AddOrderItem(Orderitems orderItem);
}