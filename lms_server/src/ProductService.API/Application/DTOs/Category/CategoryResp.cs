using ProductService.API.Domain;

namespace ProductService.API.Application.DTOs.Category;

public record GetAllCategoriesResp(
    IEnumerable<Categories> Categories,
    string Result
);

public record GetCategoryResp(
    Categories Categories,
    string Result
);

public record CreateCategoryResp(
    Categories Categories,
    string Result
);

public record UpdateCategoryResp(
    Categories Categories,
    string Result
);