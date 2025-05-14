using Microsoft.EntityFrameworkCore;
using OrderService.API.Application.Shared.Enum;
using OrderService.API.Domain;
using OrderService.API.Infrastructure.Database;

namespace OrderService.API.Infrastructure.Repository;
public class OrderRepository : IOrderRepository
{
    private readonly CampingDbContext _dbContext;
    public OrderRepository(CampingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> CreateOrder(Orders order)
    {
        await _dbContext.Orders.AddAsync(order);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Orders?> GetOrderByUserId(Guid? userId)
    {
        return await _dbContext.Orders
                    .FirstOrDefaultAsync(x => x.UserId == userId 
                                        && x.Status == OrderStatusEnum.Processing.ToString());
    }

    public async Task<bool> CheckIfExistProcessingOrder(Guid? userId)
    {
        return await _dbContext.Orders
                    .AnyAsync(x => x.UserId == userId 
                                        && x.Status == OrderStatusEnum.Processing.ToString());
    }

    public async Task<Orders?> GetOrderById(Guid? orderId)
    {
        return await _dbContext.Orders
                    .FirstOrDefaultAsync(x => x.Id == orderId 
                                        && x.Status == OrderStatusEnum.Processing.ToString());
    }

    public async Task<bool> UpdateOrder(Orders order)
    {
        _dbContext.Orders.Update(order);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}