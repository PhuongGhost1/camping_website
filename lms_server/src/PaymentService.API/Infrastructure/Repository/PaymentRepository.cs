using Microsoft.EntityFrameworkCore;
using PaymentService.API.Application.Shared.Enum;
using PaymentService.API.Domain;
using PaymentService.API.Infrastructure.Database;

namespace PaymentService.API.Infrastructure.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly CampingDbContext _dbContext;
    public PaymentRepository(CampingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> CreatePayment(Payments payment)
    {
        await _dbContext.Payments.AddAsync(payment);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Payments?> GetPaymentById(string paymentId)
    {
        return await _dbContext.Payments
            .OrderByDescending(p => p.PaidAt)
            .FirstOrDefaultAsync(p => p.TransactionId == paymentId &&
                                p.Status == PaymentStatusEnum.Pending.ToString());
    }

    public async Task<bool> UpdatePayment(Payments payment)
    {
        _dbContext.Payments.Update(payment);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Payments?> GetPaymentByOrderId(Guid orderId)
    {
        return await _dbContext.Payments
            .OrderByDescending(p => p.PaidAt)
            .FirstOrDefaultAsync(p => p.OrderId == orderId &&
                                p.Status == PaymentStatusEnum.Pending.ToString());
    }
}