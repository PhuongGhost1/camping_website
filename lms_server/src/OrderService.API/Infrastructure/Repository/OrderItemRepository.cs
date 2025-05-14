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

    public async Task<IEnumerable<Orderitems>> GetAllOrderItems(Guid orderId)
    {
        return await _dbContext.Orderitems
            .Where(x => x.OrderId == orderId)
            .ToListAsync();
    }
}