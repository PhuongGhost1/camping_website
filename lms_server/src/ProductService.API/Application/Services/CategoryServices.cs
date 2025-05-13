using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.DTOs.Category;
using ProductService.API.Application.Shared.Type;
using ProductService.API.Domain;
using ProductService.API.Infrastructure.Repository.Category;

namespace ProductService.API.Application.Services;

public interface ICategoryServices
{
    Task<IActionResult> HandleCheckCategoryExist(string name);
    Task<IActionResult> HandleGetAllCategories();
    Task<IActionResult> HandleCreateCategory(CreateCategoryReq createCategoryReq);
    Task<IActionResult> HandleUpdateCategory(UpdateCategoryReq updateCategoryReq);
}
public class CategoryServices : ICategoryServices
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryServices(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<IActionResult> HandleCreateCategory(CreateCategoryReq createCategoryReq)
    {
        try
        {
            var category = await _categoryRepo.IsCategoryExist(createCategoryReq.Name);
            if (category is not null)
            {
                return new BadRequestObjectResult("Category already exists");
            }

            var newCategory = new Categories
            {
                Name = createCategoryReq.Name,
                Description = createCategoryReq.Description,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _categoryRepo.CreateCategory(newCategory);
            if (!result)
            {
                return new BadRequestObjectResult("Failed to create category");
            }

            return SuccessResp.Created(new CreateCategoryResp(
                Categories: newCategory,
                Result: "Category created successfully"
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> HandleGetAllCategories()
    {
        try
        {
            var categories = await _categoryRepo.GetAllCategories();
            if (categories is null || !categories.Any())
            {
                return ErrorResp.NotFound("No categories found");
            }

            return SuccessResp.Ok(categories);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> HandleCheckCategoryExist(string name)
    {
        try
        {
            var category = await _categoryRepo.IsCategoryExist(name);
            if (category is null)
            {
                return new NotFoundObjectResult("Category not found");
            }

            return SuccessResp.Ok(new GetCategoryResp(
                Categories: category,
                Result: "Single Category retrieved successfully"
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> HandleUpdateCategory(UpdateCategoryReq updateCategoryReq)
    {
        try
        {
            var category = await _categoryRepo.GetCategoriesById(updateCategoryReq.Id);
            if (category is null)
            {
                return new NotFoundObjectResult("Category not found");
            }

            category.Name = updateCategoryReq.Name ?? category.Name;     
            category.Description = updateCategoryReq.Description ?? category.Description;

            var result = await _categoryRepo.UpdateCategory(category);
            if (!result)
            {
                return new BadRequestObjectResult("Failed to update category");
            }

            return SuccessResp.Ok(new UpdateCategoryResp(
                Categories: category,
                Result: "Category updated successfully"
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }
}