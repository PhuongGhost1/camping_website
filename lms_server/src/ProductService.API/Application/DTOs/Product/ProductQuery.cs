using ProductService.API.Application.DTOs.Pagination;
using ProductService.API.Application.Shared.Enum;
using ProductService.API.Domain;

namespace ProductService.API.Application.DTOs.Product;
public class GetProductReq : PaginationReq
{
    public string? SearchKeyword { get; set; }
}


public record CreateProductReq(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    IFormFile ImageUrl,
    Guid CategoryId
);

public record UpdateProductReq(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    int? Stock,
    IFormFile? ImageUrl,
    Guid? CategoryId
);