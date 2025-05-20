using PaymentService.API.Domain;

namespace PaymentService.API.Infrastructure.Repository;

public interface IPaymentRepository
{
    Task<bool> CreatePayment(Payments payment);
    Task<bool> UpdatePayment(Payments payment);
    Task<Payments?> GetPaymentById(string paymentId);
    Task<Payments?> GetPaymentByOrderId(Guid orderId);
    Task<IEnumerable<Payments>> GetPaymentsByOrderId(Guid orderId);
    Task<IEnumerable<Payments>> GetPaymentsForStastic(int year);
}