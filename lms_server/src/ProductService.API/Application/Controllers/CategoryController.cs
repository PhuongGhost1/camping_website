using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.DTOs.Category;
using ProductService.API.Application.Services;

namespace ProductService.API.Application.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ILogger<CategoryController> logger, ICategoryServices categoryServices)
        {
            _logger = logger;
            _categoryServices = categoryServices;
        }

        [HttpGet("all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            _logger.LogInformation("Get All Categories");
            return await _categoryServices.HandleGetAllCategories();
        }

        [HttpPost("create-category")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryReq createCategoryReq)
        {
            _logger.LogInformation("Create Category");
            return await _categoryServices.HandleCreateCategory(createCategoryReq);
        }

        [HttpPut("update-category")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryReq updateCategoryReq)
        {
            _logger.LogInformation("Update Category");
            return await _categoryServices.HandleUpdateCategory(updateCategoryReq);
        }

        [HttpGet("check-category-exist")]
        public async Task<IActionResult> CheckCategoryExist([FromQuery] string name)
        {
            _logger.LogInformation("Check Category Exist");
            return await _categoryServices.HandleCheckCategoryExist(name);
        }
    }
}