using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Application.DTOs.Payment;
using PaymentService.API.Application.Shared.Constant;
using PaymentService.API.Application.Shared.Enum;
using PaymentService.API.Application.Shared.Type;
using PaymentService.API.Domain;
using PaymentService.API.Infrastructure.Messaging.Publisher;
using PaymentService.API.Infrastructure.Repository;
using PayPal.Api;

namespace PaymentService.API.Application.Services;

public interface IPaymentServices
{
    Task<IActionResult> ProcessPaymentWithPaypal(ProcessPaymentReq req);
    Task<IActionResult> ConfirmPayment(ConfirmPaymentReq req);
    Task<IActionResult> GetAllPaymentByOrderId(Guid orderId);
}
public class PaymentServices : IPaymentServices
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IEventPublisher _eventPublisher;
    public PaymentServices(IPaymentRepository paymentRepository, IEventPublisher eventPublisher)
    {
        _paymentRepository = paymentRepository;
        _eventPublisher = eventPublisher;
    }

    private APIContext GetAPIContext()
    {
        var config = new Dictionary<string, string>
        {
            { "mode", PayPalConstant.MODE },
            { "clientId", PayPalConstant.CLIENT_ID },
            { "clientSecret", PayPalConstant.CLIENT_SECRET }
        };
        var accessToken = new OAuthTokenCredential(PayPalConstant.CLIENT_ID, PayPalConstant.CLIENT_SECRET).GetAccessToken();
        return new APIContext(accessToken) { Config = config };
    }

    private Payment CreatePayment(ProcessCreatePaymentReq req)
    {
        var apiContext = GetAPIContext();
        var payer = new Payer() { payment_method = "paypal" };
        var redirectUrls = new RedirectUrls()
        {
            cancel_url = req.CancelUrl,
            return_url = req.ReturnUrl
        };
        var details = new Details()
        {
            tax = "0",
            shipping = "0",
            subtotal = req.Total.ToString()
        };
        var amount = new Amount()
        {
            currency = "USD",
            total = req.Total.ToString(),
            details = details
        };
        var transactionList = new List<Transaction>();
        var transactionItem = new Transaction()
        {
            description = "Transaction description.",
            invoice_number = Guid.NewGuid().ToString(),
            amount = amount
        };
        transactionList.Add(transactionItem);
        var payment = new Payment()
        {
            intent = "sale",
            payer = payer,
            transactions = transactionList,
            redirect_urls = redirectUrls
        };
        return payment.Create(apiContext);
    }

    public async Task<IActionResult> ProcessPaymentWithPaypal(ProcessPaymentReq req)
    {
        try
        {
            string return_url = "http://localhost:5173/?payment=success";
            string cancel_url = "http://localhost:5173/?payment=fail";

            var payment = CreatePayment(new ProcessCreatePaymentReq
            {
                Total = req.Total,
                ReturnUrl = return_url,
                CancelUrl = cancel_url
            });
            var approvalUrl = payment.links.FirstOrDefault(x => x.rel.ToLower() == "approval_url")?.href;
            if (approvalUrl == null)
            {
                return ErrorResp.BadRequest("Unable to get approval URL");
            }

            var convertTotal = Math.Round(req.Total, 2);

            var paymentOrder = await _paymentRepository.GetPaymentByOrderId(req.OrderId);
            if (paymentOrder == null)
            {
                var paymentObj = new Payments
                {
                    OrderId = req.OrderId,
                    PaymentMethod = PaymentMethodEnum.Paypal.ToString(),
                    Status = PaymentStatusEnum.Pending.ToString(),
                    TransactionId = payment.id,
                    Amount = convertTotal,
                    PaidAt = DateTime.UtcNow,
                };

                var isProcess = await _paymentRepository.CreatePayment(paymentObj);
                if (!isProcess)
                    return ErrorResp.BadRequest("Unable to process payment");
            }

            return SuccessResp.Ok(new ProcessPaymentResp
            {
                ApprovalUrl = approvalUrl,
                PaymentId = payment.id
            });
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> ConfirmPayment(ConfirmPaymentReq req)
    {
        try
        {
            var apiContext = GetAPIContext();

            var paymentExecution = new PaymentExecution() { payer_id = req.PayerId };
            var payment = new Payment() { id = req.PaymentId, token = req.Token };
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            if (executedPayment.state.ToLower() != "approved")
                return ErrorResp.BadRequest("Payment not approved");

            var paymentOrder = await _paymentRepository.GetPaymentById(req.PaymentId);
            if (paymentOrder == null)
                return ErrorResp.BadRequest("Payment not found");

            _eventPublisher.PublishPaymentProcessed(paymentOrder.OrderId, paymentOrder.Amount);

            paymentOrder.TransactionId = executedPayment.id;
            paymentOrder.Status = PaymentStatusEnum.Success.ToString();

            var isProcess = await _paymentRepository.UpdatePayment(paymentOrder);
            if (!isProcess)
                return ErrorResp.BadRequest("Unable to process payment");

            return SuccessResp.Ok("Payment success");
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> GetAllPaymentByOrderId(Guid orderId)
    {
        try
        {
            var paymentOrder = await _paymentRepository.GetPaymentsByOrderId(orderId);
            if (paymentOrder == null)
                return ErrorResp.BadRequest("Payment not found");

            return SuccessResp.Ok(paymentOrder);
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}