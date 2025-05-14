using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.Application.DTOs.OrderItem;
using OrderService.API.Application.Services;

namespace OrderService.API.Application.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderServices _orderService;
        private readonly IOrderItemServices _orderItemService;
        public OrderController(ILogger<OrderController> logger, IOrderServices orderService, IOrderItemServices orderItemService)
        {
            _logger = logger;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        [Authorize]
        [HttpGet("order")]
        public async Task<IActionResult> GetOrderByUserId()
        {
            _logger.LogInformation("Get order by user id");
            var userId = Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            return await _orderService.GetOrderByUserId(userId);
        }

        [Authorize]
        [HttpGet("all-order-products")]
        public async Task<IActionResult> GetOrderProducts([FromQuery] Guid orderId)
        {
            _logger.LogInformation("Get order products by order id");
            return await _orderItemService.GetOrderProducts(orderId);
        }

        [Authorize]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddOrderItem([FromBody] AddToCartRequest req)
        {
            _logger.LogInformation("Add product to order");
            return await _orderItemService.AddOrderItem(req);
        }
    }
}