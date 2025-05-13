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

    public async Task<bool> CreateProduct(Products product)
    {
        await _dbContext.Products.AddAsync(product);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<Products?> GetProductById(Guid id)
    {
        return await _dbContext.Products
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProductList> GetProducts(GetProductReq req)
    {
        string searchKeyword = req.SearchKeyword ?? "";

        var query = _dbContext.Products
                                .Include(p => p.Reviews)
                                .Include(p => p.Category)
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

    public async Task<bool> UpdateProduct(Products product)
    {
        _dbContext.Products.Update(product);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
