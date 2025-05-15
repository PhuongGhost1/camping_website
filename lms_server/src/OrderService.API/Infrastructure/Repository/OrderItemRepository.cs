using Microsoft.EntityFrameworkCore;
using OrderService.API.Domain;
using OrderService.API.Infrastructure.Database;

namespace OrderService.API.Infrastructure.Repository;
public class OrderItemRepository : IOrderItemRepository
{
    private readonly CampingDbContext _dbContext;
    public OrderItemRepository(CampingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddOrderItem(Orderitems orderItem)
    {
        await _dbContext.Orderitems.AddAsync(orderItem);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<int> DeleteOrderItem(Guid orderItemId)
    {
        return await _dbContext.Orderitems
            .Where(x => x.Id == orderItemId)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Orderitems>> GetAllOrderItems(Guid orderId)
    {
        return await _dbContext.Orderitems
            .Where(x => x.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<Orderitems?> GetOrderItemById(Guid productId, Guid orderId)
    {
        return await _dbContext.Orderitems
            .FirstOrDefaultAsync(x => x.ProductId == productId && x.OrderId == orderId);
    }

    public async Task<bool> UpdateOrderItem(Orderitems orderItem)
    {
        _dbContext.Orderitems.Update(orderItem);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}