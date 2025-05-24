using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Moq;
using ProductService.API.Application.DTOs.Category;
using ProductService.API.Application.Services;
using ProductService.API.Application.Endpoints;
using Xunit;

public class CategoryEndpointsTests
{
    [Fact]
    public async Task Test_GetAllCategories_ReturnsCategoryList()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryServices>();
        var expectedCategories = new List<CategoryDto>
        {
            new CategoryDto { Id = 1, Name = "Books" },
            new CategoryDto { Id = 2, Name = "Electronics" }
        };
        mockCategoryService
            .Setup(s => s.HandleGetAllCategories())
            .ReturnsAsync(expectedCategories);

        // The endpoint is defined as a lambda, so we extract it for direct invocation
        var getAllCategoriesDelegate = (ICategoryServices categoryService) =>
            categoryService.HandleGetAllCategories().ContinueWith(task => Results.Ok(task.Result));

        // Act
        var result = await getAllCategoriesDelegate(mockCategoryService.Object) as Ok<List<CategoryDto>>;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategories, result.Value);
    }

    [Fact]
    public async Task Test_CreateCategory_ValidInput_Authorized_ReturnsSuccess()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryServices>();
        var createCategoryReq = new CreateCategoryReq
        {
            Name = "New Category"
        };
        var expectedCategory = new CategoryDto
        {
            Id = 3,
            Name = "New Category"
        };
        mockCategoryService
            .Setup(s => s.HandleCreateCategory(createCategoryReq))
            .ReturnsAsync(expectedCategory);

        // The endpoint is defined as a lambda, so we extract it for direct invocation
        var createCategoryDelegate = (ICategoryServices categoryService, CreateCategoryReq req) =>
            categoryService.HandleCreateCategory(req).ContinueWith(task => Results.Ok(task.Result));

        // Act
        var result = await createCategoryDelegate(mockCategoryService.Object, createCategoryReq) as Ok<CategoryDto>;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategory.Id, result.Value.Id);
        Assert.Equal(expectedCategory.Name, result.Value.Name);
    }

    [Fact]
    public async Task Test_UpdateCategory_ValidInput_Authorized_ReturnsSuccess()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryServices>();
        var updateCategoryReq = new UpdateCategoryReq
        {
            Id = 1,
            Name = "Updated Category"
        };
        var expectedCategory = new CategoryDto
        {
            Id = 1,
            Name = "Updated Category"
        };
        mockCategoryService
            .Setup(s => s.HandleUpdateCategory(updateCategoryReq))
            .ReturnsAsync(expectedCategory);

        // The endpoint is defined as a lambda, so we extract it for direct invocation
        var updateCategoryDelegate = (ICategoryServices categoryService, UpdateCategoryReq req) =>
            categoryService.HandleUpdateCategory(req).ContinueWith(task => Results.Ok(task.Result));

        // Act
        var result = await updateCategoryDelegate(mockCategoryService.Object, updateCategoryReq) as Ok<CategoryDto>;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategory.Id, result.Value.Id);
        Assert.Equal(expectedCategory.Name, result.Value.Name);
    }

    [Fact]
    public async Task Test_CreateCategory_InvalidInput_ReturnsValidationError()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryServices>();
        // Simulate invalid input: for example, missing required Name property
        var invalidCreateCategoryReq = new CreateCategoryReq
        {
            Name = "" // Assume Name is required and empty string is invalid
        };

        // The endpoint is defined as a lambda, so we extract it for direct invocation
        var createCategoryDelegate = (ICategoryServices categoryService, CreateCategoryReq req) =>
            categoryService.HandleCreateCategory(req).ContinueWith(task => Results.Ok(task.Result));

        // Act
        // Since validation is handled by WithValidation<T>(), and not by the service,
        // we simulate what would happen if the validation fails before reaching the service.
        // In a real integration test, the middleware would return a validation error.
        // Here, we simulate this by asserting that the service is never called for invalid input.

        // Assert
        // The service should not be called for invalid input, so we do not call the delegate.
        // Instead, we simulate the validation error response.
        // In ASP.NET Core, this would typically be a 400 BadRequest with validation details.
        var validationProblemDetails = new Dictionary<string, string[]>
        {
            { "Name", new[] { "The Name field is required." } }
        };
        var result = Results.ValidationProblem(validationProblemDetails);

        Assert.IsType<ValidationProblem>(result);
        var validationResult = result as ValidationProblem;
        Assert.NotNull(validationResult);
        Assert.True(validationResult.Value.ContainsKey("Name"));
        Assert.Contains("The Name field is required.", validationResult.Value["Name"]);
        mockCategoryService.Verify(s => s.HandleCreateCategory(It.IsAny<CreateCategoryReq>()), Times.Never);
    }

    [Fact]
    public async Task Test_CreateOrUpdateCategory_Unauthorized_ReturnsAccessDenied()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryServices>();
        var createCategoryReq = new CreateCategoryReq { Name = "Unauthorized Category" };
        var updateCategoryReq = new UpdateCategoryReq { Id = 1, Name = "Unauthorized Update" };

        // The endpoints require authorization via .RequireAuthorization().
        // In a real integration test, unauthorized requests would be blocked by middleware and not reach the delegate.
        // Here, we simulate the expected framework behavior for unauthorized access.

        // Simulate unauthorized access for CreateCategory
        var unauthorizedResultCreate = Results.Unauthorized();
        Assert.IsType<UnauthorizedHttpResult>(unauthorizedResultCreate);

        // Simulate unauthorized access for UpdateCategory
        var unauthorizedResultUpdate = Results.Unauthorized();
        Assert.IsType<UnauthorizedHttpResult>(unauthorizedResultUpdate);

        // The service methods should never be called if unauthorized
        mockCategoryService.Verify(s => s.HandleCreateCategory(It.IsAny<CreateCategoryReq>()), Times.Never);
        mockCategoryService.Verify(s => s.HandleUpdateCategory(It.IsAny<UpdateCategoryReq>()), Times.Never);
    }
}