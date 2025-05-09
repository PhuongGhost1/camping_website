using ProductService.API.Application.DTOs.Pagination;
using ProductService.API.Domain;

namespace ProductService.API.Application.DTOs.Product;
public class ProductResp : PaginationResp
{
    public IEnumerable<Products> Products { get; set; } = null!;
}
