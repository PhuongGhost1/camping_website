using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.DTOs.Product;
using ProductService.API.Application.Shared.Enum;
using ProductService.API.Application.Shared.Type;
using ProductService.API.Core.GCSService;
using ProductService.API.Domain;
using ProductService.API.Infrastructure.Cache;
using ProductService.API.Infrastructure.Repository.Category;
using ProductService.API.Infrastructure.Repository.Product;

namespace ProductService.API.Application.Services;
public interface IProductService
{
    public Task<IActionResult> HandleGetProducts(GetProductReq req, Guid? userId);
    public Task<IActionResult> HandleCreateProduct(CreateProductReq createProductReq);
    public Task<IActionResult> HandleUpdateProduct(UpdateProductReq updateProductReq);
}

public class ProductServices : IProductService
{
    private readonly IProductRepository _productRepo;
    private readonly ICacheService _cacheService;
    private readonly IMediaServices _mediaServices;
    private readonly ICategoryRepository _categoryRepo;
    public ProductServices(IProductRepository productRepo, ICategoryRepository categoryRepo, ICacheService cacheService, IMediaServices mediaServices)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _cacheService = cacheService;
        _mediaServices = mediaServices;
    }

    public async Task<IActionResult> HandleCreateProduct(CreateProductReq createProductReq)
    {
        try
        {
            if (createProductReq is null)
            {
                return ErrorResp.InternalServerError("Request is required");
            }

            var category = await _categoryRepo.GetCategoriesById(createProductReq.CategoryId);
            if (category is null)
            {
                return ErrorResp.InternalServerError("Category not found");
            }

            var newProduct = new Products
            {
                Name = createProductReq.Name,
                Description = createProductReq.Description,
                Price = createProductReq.Price,
                Stock = createProductReq.Stock,
                ImageUrl = _mediaServices.HandleCreateMedia(createProductReq.ImageUrl, MediaTypeEnum.Image).Result,
                CategoryId = createProductReq.CategoryId
            };

            var result = await _productRepo.CreateProduct(newProduct);
            if (!result)
            {
                return ErrorResp.InternalServerError("Failed to create product");
            }

            return SuccessResp.Created(new CreateProductResp (
                Product: newProduct,
                Result: "Product created successfully"
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> HandleGetProducts(GetProductReq req, Guid? userId)
    {
        try
        {
            var redisKey = $"products:{userId}:{req.Page}:{req.PageSize}";

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
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> HandleUpdateProduct(UpdateProductReq updateProductReq)
    {
        try
        {
            if (updateProductReq.Id == Guid.Empty || updateProductReq.Id == null)
            {
                return ErrorResp.InternalServerError("Product ID is required");
            }

            var product = await _productRepo.GetProductById(updateProductReq.Id);

            if (product is null)
                return ErrorResp.InternalServerError("Product not found");

            if(updateProductReq.CategoryId is not null)
            {
                var category = await _categoryRepo.GetCategoriesById(updateProductReq.CategoryId);
                if (category is null)
                {
                    return ErrorResp.InternalServerError("Category not found");
                }
            }

            product.Name = updateProductReq.Name ?? product.Name;
            product.Description = updateProductReq.Description ?? product.Description;
            product.Price = updateProductReq.Price ?? product.Price;
            product.Stock = updateProductReq.Stock ?? product.Stock;

            product.ImageUrl = updateProductReq.ImageUrl is not null ? _mediaServices.HandleUploadMedia(updateProductReq.ImageUrl, MediaTypeEnum.Image).Result : product.ImageUrl;

            product.CategoryId = updateProductReq.CategoryId ?? product.CategoryId;

            var result = await _productRepo.UpdateProduct(product);
            if (!result)
            {
                return ErrorResp.InternalServerError("Failed to update product");
            }

            return SuccessResp.Ok(new UpdateProductResp(
                Product: product,
                Result: "Product updated successfully"
            ));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
