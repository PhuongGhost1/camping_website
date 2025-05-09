using Microsoft.EntityFrameworkCore;
using ProductService.API.Application.DTOs.Product;
using ProductService.API.Domain;
using ProductService.API.Infrastructure.Database;

namespace ProductService.API.Infrastructure.Repository.Product;
public class ProductRepository : IProductRepository
{
    private readonly CampingDbContext _dbContext;

    public ProductRepository(CampingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductList> GetProducts(GetProductReq req)
    {
        string searchKeyword = req.SearchKeyword ?? "";

        var query = _dbContext.Products
                                .Include(p => p.Reviews)
                                .Where(p => p.Name.Contains(searchKeyword))
                                .AsEnumerable();

        var totalCount = query.Count();

        var products = query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((req.Page - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ToList();

        return new ProductList
        {
            Products = products,
            TotalCount = totalCount
        };
    }
}
