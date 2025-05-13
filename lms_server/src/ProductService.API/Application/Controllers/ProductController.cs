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

        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetProductReq req)
        {
            _logger.LogInformation("Get Products");
            Guid? userId = null;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedId))
            {
                userId = parsedId;
            }
            return await _productService.HandleGetProducts(req, userId);
        }

        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductReq createProductReq)
        {
            _logger.LogInformation("Create Product");
            return await _productService.HandleCreateProduct(createProductReq);
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductReq updateProductReq)
        {
            _logger.LogInformation("Update Product");
            return await _productService.HandleUpdateProduct(updateProductReq);
        }
    }
}
