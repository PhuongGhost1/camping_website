using ProductService.API.Application.DTOs.Pagination;

namespace ProductService.API.Application.DTOs.Product;
public class GetProductReq : PaginationReq
{
    public string? SearchKeyword { get; set; }
}
