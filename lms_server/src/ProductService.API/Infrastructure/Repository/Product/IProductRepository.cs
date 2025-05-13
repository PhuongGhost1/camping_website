using ProductService.API.Application.DTOs.Product;
using ProductService.API.Domain;

namespace ProductService.API.Infrastructure.Repository.Product;
public class ProductList
{
    public IEnumerable<Products> Products { get; set; } = new List<Products>();
    public int TotalCount { get; set; }
}
public interface IProductRepository
{
    Task<ProductList> GetProducts(GetProductReq req);
    Task<Products?> GetProductById(Guid id);
    Task<bool> CreateProduct(Products product);
    Task<bool> UpdateProduct(Products product);
}
