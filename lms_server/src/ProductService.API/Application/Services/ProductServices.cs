using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.DTOs.Product;
using ProductService.API.Application.Shared.Type;
using ProductService.API.Domain;
using ProductService.API.Infrastructure.Cache;
using ProductService.API.Infrastructure.Repository.Product;

namespace ProductService.API.Application.Services;
public interface IProductService
{
    public Task<IActionResult> HandleGetProducts(GetProductReq req, Guid? userId);
}

public class ProductServices : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICacheService _cacheService;
    public ProductServices(IProductRepository productRepo, ICacheService cacheService)
    {
        _productRepo = productRepo; 
        _cacheService = cacheService;
    }
    public async Task<IActionResult> HandleGetProducts(GetProductReq req, Guid? userId)
    {
        try
        {
            var redisKey = $"products:{userId}";

            var productsCache = await _cacheService.Get<ProductResp>(redisKey);
            if (productsCache is not null)
            {
                return SuccessResp.Ok(productsCache);
            }

            var products = await _productRepo.GetProducts(req);

            if (products is null || products.TotalCount == 0) return ErrorResp.InternalServerError("Products are empty");

            var resp =  new ProductResp
            {
                Products = products.Products,
                Page = req.Page,
                PageSize = req.PageSize,
                Total = products.TotalCount
            };

            await _cacheService.Set(redisKey, resp);

            return SuccessResp.Ok(resp);
        }
        catch(Exception ex)
        {
            throw;
        }
    }
}
