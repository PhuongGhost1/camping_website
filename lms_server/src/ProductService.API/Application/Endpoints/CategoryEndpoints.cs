using ProductService.API.Application.DTOs.Category;
using ProductService.API.Application.Services;
using ProductService.API.Application.Validators;

namespace ProductService.API.Application.Endpoints;
public static class CategoryEndpoints
{
    public static RouteGroupBuilder MapCategoryEndpoints(this RouteGroupBuilder group)
    {
        var categoryGroup = group.WithTags("Category");

        categoryGroup.MapGet("/all-categories", async (
            ICategoryServices categoryService
        ) =>
        {
            var result = await categoryService.HandleGetAllCategories();
            return Results.Ok(result);
        })
        .WithName("GetCategories");

        categoryGroup.MapPost("/create-category", async (
            ICategoryServices categoryService,
            CreateCategoryReq createCategoryReq
        ) =>
        {
            var result = await categoryService.HandleCreateCategory(createCategoryReq);
            return Results.Ok(result);
        })
        .WithName("CreateCategory")
        .RequireAuthorization()
        .WithValidation<CreateCategoryReq>();

        categoryGroup.MapPut("/update-category", async (
            ICategoryServices categoryService,
            UpdateCategoryReq updateCategoryReq
        ) =>
        {
            var result = await categoryService.HandleUpdateCategory(updateCategoryReq);
            return Results.Ok(result);
        })
        .WithName("UpdateCategory")
        .RequireAuthorization()
        .WithValidation<UpdateCategoryReq>();

        categoryGroup.MapGet("/check-category-exist", async (
            ICategoryServices categoryService,
            string name
        ) =>
        {
            var result = await categoryService.HandleCheckCategoryExist(name);
            return Results.Ok(result);
        })
        .WithName("CheckCategoryExist");

        return categoryGroup;
    }
}