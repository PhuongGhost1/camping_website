namespace ProductService.API.Application.DTOs.Category;

public record CreateCategoryReq(
    string Name,
    string Description
);

public record UpdateCategoryReq(
    Guid Id,
    string? Name,
    string? Description
);