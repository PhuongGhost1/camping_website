using Microsoft.AspNetCore.Mvc;
using ProductService.API.Application.Bindings;
using ProductService.API.Application.DTOs.Product;
using ProductService.API.Application.Services;
using ProductService.API.Application.Validators;

namespace ProductService.API.Application.Endpoints;
public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this RouteGroupBuilder group)
    {
        var productGroup = group.WithTags("Product");

        productGroup.MapGet("/all-products", async (
        IProductService productService,
        [AsParameters] GetProductReq req,
        UserId? userId
        ) =>
        {
            Guid? parsedUserId = userId?.Value ?? null;
            var result = await productService.HandleGetProducts(req, parsedUserId);
            return Results.Ok(result);
        })
        .WithName("GetProducts")
        .WithValidation<GetProductReq>();

        productGroup.MapPost("/create-product", async (
        IProductService productService,
        HttpRequest request
        ) =>
        {
            
            var createProductReq = await ParseProductReq.ParseCreateProductReq(request); 
            var result = await productService.HandleCreateProduct(createProductReq);
            return Results.Ok(result);
        })
        .WithName("CreateProduct")
        .RequireAuthorization()
        .WithValidation<CreateProductReq>();

        productGroup.MapPut("/update-product", async (
        IProductService productService,
        HttpRequest request
        ) =>
        {
            var updateProductReq = await ParseProductReq.ParseUpdateProductReq(request);
            var result = await productService.HandleUpdateProduct(updateProductReq);
            return Results.Ok(result);
        })
        .WithName("UpdateProduct")
        .RequireAuthorization()
        .WithValidation<UpdateProductReq>();

        productGroup.MapGet("/{id:guid}", async (IProductService productService, Guid id) =>
        {
            var result = await productService.HandleGetProductById(id);
            return Results.Ok(result);
        })
        .WithName("GetProductById");

        productGroup.MapPost("/upload-product", async (
        IProductService productService,
        HttpRequest request
        ) =>
        {
            var createProductReq = await ParseProductReq.ParseCreateProductReq(request);
            var result = await productService.HandleUploadProduct(createProductReq);
            return Results.Ok(result);
        })
        .WithName("UploadProduct")
        .RequireAuthorization()
        .WithValidation<CreateProductReq>();

        return productGroup;
    }
}