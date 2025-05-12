using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.DTOs.Product;
using ProductService.API.Application.Services;

namespace ProductService.API.Application.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;   
        }

        [Authorize]
        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetProductReq req)
        {
            _logger.LogInformation("Get Products");
            var userId = Guid.Parse(User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value);
            return await _productService.HandleGetProducts(req, userId);
        }
    }
}
