// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using PaymentService.API.Application.DTOs.Payment;
// using PaymentService.API.Application.Services;
// namespace PaymentService.API.Application.Controllers
// {
//     [ApiController]
//     [Route("api/payments")]
//     public class PaymentController : ControllerBase
//     {
//         private readonly IPaymentServices _paymentServices;
//         public PaymentController(IPaymentServices paymentServices)
//         {
//             _paymentServices = paymentServices;
//         }

//         [Authorize]
//         [HttpPost("process-payment")]
//         public async Task<IActionResult> ProcessPaymentWithPaypal([FromBody] ProcessPaymentReq req)
//         {
//             return await _paymentServices.ProcessPaymentWithPaypal(req);
//         }

//         [Authorize]
//         [HttpGet("confirm-payment")]
//         public async Task<IActionResult> ConfirmPayment([FromQuery] ConfirmPaymentReq req)
//         {
//             return await _paymentServices.ConfirmPayment(req);
//         }

//         [Authorize]
//         [HttpGet("all-payments")]
//         public async Task<IActionResult> GetAllPaymentByOrderId([FromQuery] Guid orderId)
//         {
//             return await _paymentServices.GetAllPaymentByOrderId(orderId);
//         }

//         [HttpGet("stastic-payment-in-year")]
//         public async Task<IActionResult> StasticPaymentInYear()
//         {
//             return await _paymentServices.StasticPaymentInYear(2025);
//         }
//     }
// }