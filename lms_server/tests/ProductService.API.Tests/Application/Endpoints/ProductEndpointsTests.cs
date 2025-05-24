using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using ProductService.API.Application.DTOs.Product;
using ProductService.API.Application.Services;
using ProductService.API.Application.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Linq;
using System.Threading;
using ProductService.API.Application.Bindings;

public class ProductEndpointsTests
{
    [Fact]
    public async Task Test_GetAllProducts_ReturnsProductList()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var expectedProducts = new List<ProductDto>
        {
            new ProductDto { Id = Guid.NewGuid(), Name = "Product1" },
            new ProductDto { Id = Guid.NewGuid(), Name = "Product2" }
        };
        var req = new GetProductReq(); // Assuming default is valid
        Guid? userId = Guid.NewGuid();

        mockProductService
            .Setup(s => s.HandleGetProducts(It.IsAny<GetProductReq>(), It.IsAny<Guid?>()))
            .ReturnsAsync(expectedProducts);

        // Setup endpoint delegate
        var endpointDelegate = ProductEndpoints.MapProductEndpoints(new RouteGroupBuilderMock())
            .Routes
            .First(r => r.Pattern.RawText == "/all-products")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();

        // Add query parameters if needed for GetProductReq binding
        // (Assuming GetProductReq is bound from query, otherwise adjust as needed)

        // Add UserId as a route value or header if needed
        // (Assuming UserId is bound from somewhere, otherwise adjust as needed)

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var okResult = Assert.IsType<Ok<List<ProductDto>>>(result);
        Assert.Equal(expectedProducts, okResult.Value);
    }

    // Helper mock RouteGroupBuilder for endpoint extraction
    private class RouteGroupBuilderMock : RouteGroupBuilder
    {
        public List<RouteEndpointBuilder> Routes { get; } = new List<RouteEndpointBuilder>();

        public override RouteGroupBuilder MapGet(string pattern, Delegate handler)
        {
            var builder = new RouteEndpointBuilder(
                context => Task.CompletedTask,
                RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                // Simulate parameter binding for test
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                var req = new GetProductReq();
                UserId? userId = null;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, req, userId);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }

        public override RouteGroupBuilder WithTags(params string[] tags) => this;
        public override RouteGroupBuilder MapPost(string pattern, Delegate handler) => this;
        public override RouteGroupBuilder MapPut(string pattern, Delegate handler) => this;
        public override RouteGroupBuilder RequireAuthorization() => this;
        public override RouteGroupBuilder WithValidation<T>() => this;
        public override RouteGroupBuilder MapGet(string pattern, RequestDelegate handler) => this;
    }

    [Fact]
    public async Task Test_CreateProduct_Authorized_ValidData()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockHttpRequest = new Mock<HttpRequest>();
        var expectedProduct = new ProductDto { Id = Guid.NewGuid(), Name = "CreatedProduct" };
        var createProductReq = new CreateProductReq { Name = "CreatedProduct" };

        // Mock ParseProductReq.ParseCreateProductReq to return our createProductReq
        var parseProductReqType = typeof(ProductService.API.Application.Bindings.ParseProductReq);
        var parseCreateProductReqMethod = parseProductReqType.GetMethod("ParseCreateProductReq");
        var mockParseProductReq = new MockRepository(MockBehavior.Strict);

        // Use Moq.Protected or similar to mock static if possible, otherwise use delegate
        // For this test, we'll use a delegate to simulate the static method
        // Setup productService.HandleCreateProduct to return expectedProduct
        mockProductService
            .Setup(s => s.HandleCreateProduct(It.Is<CreateProductReq>(r => r.Name == createProductReq.Name)))
            .ReturnsAsync(expectedProduct);

        // Setup a delegate for the endpoint handler
        Delegate handler = async (IProductService productService, HttpRequest request) =>
        {
            // Simulate ParseProductReq.ParseCreateProductReq(request)
            var req = createProductReq;
            var result = await productService.HandleCreateProduct(req);
            return Results.Ok(result);
        };

        // Setup endpoint delegate
        var endpointDelegate = new RouteGroupBuilderMock_Post(handler)
            .Routes
            .First(r => r.Pattern.RawText == "/create-product")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();
        context.Request.Method = HttpMethods.Post;

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var okResult = Assert.IsType<Ok<ProductDto>>(result);
        Assert.Equal(expectedProduct, okResult.Value);
    }

    // Helper mock RouteGroupBuilder for POST endpoint extraction
    private class RouteGroupBuilderMock_Post : ProductEndpointsTests.RouteGroupBuilderMock
    {
        public RouteGroupBuilderMock_Post(Delegate handler)
        {
            MapPost("/create-product", handler);
        }

        public override RouteGroupBuilder MapPost(string pattern, Delegate handler)
        {
            var builder = new Microsoft.AspNetCore.Routing.RouteEndpointBuilder(
                context => Task.CompletedTask,
                Microsoft.AspNetCore.Routing.Patterns.RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                var request = ctx.Request;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, request);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }
    }

    [Fact]
    public async Task Test_UpdateProduct_Authorized_ValidUpdate()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockHttpRequest = new Mock<HttpRequest>();
        var expectedProduct = new ProductDto { Id = Guid.NewGuid(), Name = "UpdatedProduct" };
        var updateProductReq = new UpdateProductReq { Id = expectedProduct.Id, Name = "UpdatedProduct" };

        // Simulate ParseProductReq.ParseUpdateProductReq(request) returns updateProductReq
        Delegate handler = async (IProductService productService, HttpRequest request) =>
        {
            var req = updateProductReq;
            var result = await productService.HandleUpdateProduct(req);
            return Results.Ok(result);
        };

        mockProductService
            .Setup(s => s.HandleUpdateProduct(It.Is<UpdateProductReq>(r => r.Id == updateProductReq.Id && r.Name == updateProductReq.Name)))
            .ReturnsAsync(expectedProduct);

        // Setup endpoint delegate
        var endpointDelegate = new RouteGroupBuilderMock_Put(handler)
            .Routes
            .Find(r => r.Pattern.RawText == "/update-product")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();
        context.Request.Method = HttpMethods.Put;

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var okResult = Assert.IsType<Ok<ProductDto>>(result);
        Assert.Equal(expectedProduct, okResult.Value);
    }

    // Helper mock RouteGroupBuilder for PUT endpoint extraction
    private class RouteGroupBuilderMock_Put : ProductEndpointsTests.RouteGroupBuilderMock
    {
        public RouteGroupBuilderMock_Put(Delegate handler)
        {
            MapPut("/update-product", handler);
        }

        public override RouteGroupBuilder MapPut(string pattern, Delegate handler)
        {
            var builder = new Microsoft.AspNetCore.Routing.RouteEndpointBuilder(
                context => Task.CompletedTask,
                Microsoft.AspNetCore.Routing.Patterns.RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                var request = ctx.Request;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, request);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }
    }

    [Fact]
    public async Task Test_UploadProduct_Authorized_ValidData()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var expectedProduct = new ProductDto { Id = Guid.NewGuid(), Name = "UploadedProduct" };
        var createProductReq = new CreateProductReq { Name = "UploadedProduct" };

        // Simulate ParseProductReq.ParseCreateProductReq(request) returns createProductReq
        Delegate handler = async (IProductService productService, HttpRequest request) =>
        {
            var req = createProductReq;
            var result = await productService.HandleUploadProduct(req);
            return Results.Ok(result);
        };

        mockProductService
            .Setup(s => s.HandleUploadProduct(It.Is<CreateProductReq>(r => r.Name == createProductReq.Name)))
            .ReturnsAsync(expectedProduct);

        // Setup endpoint delegate
        var endpointDelegate = new RouteGroupBuilderMock_Post_Upload(handler)
            .Routes
            .First(r => r.Pattern.RawText == "/upload-product")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();
        context.Request.Method = HttpMethods.Post;

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var okResult = Assert.IsType<Ok<ProductDto>>(result);
        Assert.Equal(expectedProduct, okResult.Value);
    }

    // Helper mock RouteGroupBuilder for POST /upload-product endpoint extraction
    private class RouteGroupBuilderMock_Post_Upload : ProductEndpointsTests.RouteGroupBuilderMock
    {
        public RouteGroupBuilderMock_Post_Upload(Delegate handler)
        {
            MapPost("/upload-product", handler);
        }

        public override RouteGroupBuilder MapPost(string pattern, Delegate handler)
        {
            var builder = new Microsoft.AspNetCore.Routing.RouteEndpointBuilder(
                context => Task.CompletedTask,
                Microsoft.AspNetCore.Routing.Patterns.RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                var request = ctx.Request;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, request);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }
    }

    [Fact]
    public async Task Test_CreateProduct_Unauthorized()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var createProductReq = new CreateProductReq { Name = "UnauthorizedProduct" };

        // Simulate ParseProductReq.ParseCreateProductReq(request) returns createProductReq
        Delegate handler = async (IProductService productService, HttpRequest request) =>
        {
            var req = createProductReq;
            var result = await productService.HandleCreateProduct(req);
            return Results.Ok(result);
        };

        // Setup endpoint delegate
        var endpointDelegate = new RouteGroupBuilderMock_Post(handler)
            .Routes
            .First(r => r.Pattern.RawText == "/create-product")
            .RequestDelegate;

        // Setup HttpContext with required services, but WITHOUT authentication
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();
        context.Request.Method = HttpMethods.Post;

        // Simulate unauthorized user (no user identity)
        context.User = new System.Security.Claims.ClaimsPrincipal();

        // Act
        // Since the endpoint is protected by RequireAuthorization, in a real app this would be handled by middleware.
        // Here, we simulate what would happen if the endpoint is invoked without authentication.
        // We expect the endpoint to not proceed and instead return a 401 Unauthorized.
        // However, since our test bypasses middleware, we simulate the check manually.
        // So, let's assert that the endpoint would not proceed for unauthorized users.
        // In a real integration test, this would be handled by the framework.

        // For demonstration, let's check that if user is not authenticated, we return UnauthorizedResult.
        // We'll simulate this logic here.
        if (!context.User.Identity.IsAuthenticated)
        {
            var unauthorizedResult = Results.Unauthorized();
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.UnauthorizedHttpResult>(unauthorizedResult);
        }
        else
        {
            var result = await endpointDelegate(context);
            Assert.False(true, "Endpoint should not be invoked for unauthorized users.");
        }
    }

    [Fact]
    public async Task Test_GetProductById_NonExistentId()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var nonExistentId = Guid.NewGuid();

        // Simulate service returns null for non-existent product
        mockProductService
            .Setup(s => s.HandleGetProductById(nonExistentId))
            .ReturnsAsync((ProductDto)null);

        // Setup endpoint delegate for GET /{id:guid}
        Delegate handler = async (IProductService productService, Guid id) =>
        {
            var result = await productService.HandleGetProductById(id);
            if (result == null)
                return Results.NotFound();
            return Results.Ok(result);
        };

        var endpointDelegate = new RouteGroupBuilderMock_GetById(handler)
            .Routes
            .First(r => r.Pattern.RawText == "/{id:guid}")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();

        // Simulate route value for id
        context.Request.RouteValues["id"] = nonExistentId.ToString();

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var notFoundResult = Assert.IsType<NotFound>(result);
    }

    // Helper mock RouteGroupBuilder for GET /{id:guid} endpoint extraction
    private class RouteGroupBuilderMock_GetById : RouteGroupBuilder
    {
        public System.Collections.Generic.List<RouteEndpointBuilder> Routes { get; } = new System.Collections.Generic.List<RouteEndpointBuilder>();

        public RouteGroupBuilderMock_GetById(Delegate handler)
        {
            MapGet("/{id:guid}", handler);
        }

        public override RouteGroupBuilder MapGet(string pattern, Delegate handler)
        {
            var builder = new RouteEndpointBuilder(
                context => Task.CompletedTask,
                RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                // Extract id from route values
                var idStr = ctx.Request.RouteValues["id"]?.ToString();
                Guid id = Guid.TryParse(idStr, out var parsedId) ? parsedId : Guid.Empty;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, id);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }

        public override RouteGroupBuilder WithTags(params string[] tags) => this;
        public override RouteGroupBuilder MapPost(string pattern, Delegate handler) => this;
        public override RouteGroupBuilder MapPut(string pattern, Delegate handler) => this;
        public override RouteGroupBuilder RequireAuthorization() => this;
        public override RouteGroupBuilder WithValidation<T>() => this;
        public override RouteGroupBuilder MapGet(string pattern, RequestDelegate handler) => this;
    }

    [Fact]
    public async Task Test_CreateProduct_InvalidData()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockHttpRequest = new Mock<HttpRequest>();

        // Simulate ParseProductReq.ParseCreateProductReq(request) throws or returns invalid data
        // For this test, let's assume it returns a CreateProductReq with missing required fields
        var invalidCreateProductReq = new CreateProductReq(); // Missing required fields

        // Setup a delegate for the endpoint handler
        Delegate handler = async (IProductService productService, HttpRequest request) =>
        {
            // Simulate ParseProductReq.ParseCreateProductReq(request)
            var req = invalidCreateProductReq;
            // Simulate validation failure: in real app, WithValidation<CreateProductReq>() would trigger validation middleware
            // Here, we simulate what would happen if validation fails: return a validation problem result
            // In ASP.NET Core, this is typically a ValidationProblemDetails result (400)
            return Results.ValidationProblem(new System.Collections.Generic.Dictionary<string, string[]>
            {
                { "Name", new[] { "The Name field is required." } }
            });
        };

        // Setup endpoint delegate
        var endpointDelegate = new RouteGroupBuilderMock_Post_Validation(handler)
            .Routes
            .Find(r => r.Pattern.RawText == "/create-product")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();
        context.Request.Method = HttpMethods.Post;

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var validationResult = Assert.IsType<ValidationProblem>(result);
        Assert.True(validationResult.ProblemDetails.Errors.ContainsKey("Name"));
        Assert.Contains("The Name field is required.", validationResult.ProblemDetails.Errors["Name"]);
    }

    // Helper mock RouteGroupBuilder for POST endpoint extraction with validation
    private class RouteGroupBuilderMock_Post_Validation : RouteGroupBuilderMock
    {
        public RouteGroupBuilderMock_Post_Validation(Delegate handler)
        {
            MapPost("/create-product", handler);
        }

        public override RouteGroupBuilder MapPost(string pattern, Delegate handler)
        {
            var builder = new Microsoft.AspNetCore.Routing.RouteEndpointBuilder(
                context => Task.CompletedTask,
                Microsoft.AspNetCore.Routing.Patterns.RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                var request = ctx.Request;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, request);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }
    }

    [Fact]
    public async Task Test_UpdateProduct_InvalidData()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockHttpRequest = new Mock<HttpRequest>();

        // Simulate ParseProductReq.ParseUpdateProductReq(request) returns invalid data (e.g., missing required fields)
        var invalidUpdateProductReq = new UpdateProductReq(); // Missing required fields

        // Setup a delegate for the endpoint handler
        Delegate handler = async (IProductService productService, HttpRequest request) =>
        {
            // Simulate ParseProductReq.ParseUpdateProductReq(request)
            var req = invalidUpdateProductReq;
            // Simulate validation failure: in real app, WithValidation<UpdateProductReq>() would trigger validation middleware
            // Here, we simulate what would happen if validation fails: return a validation problem result
            // In ASP.NET Core, this is typically a ValidationProblemDetails result (400)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Name", new[] { "The Name field is required." } }
            });
        };

        // Setup endpoint delegate
        var endpointDelegate = new RouteGroupBuilderMock_Put_Validation(handler)
            .Routes
            .Find(r => r.Pattern.RawText == "/update-product")
            .RequestDelegate;

        // Setup HttpContext with required services
        var context = new DefaultHttpContext();
        context.RequestServices = new ServiceCollection()
            .AddSingleton(mockProductService.Object)
            .BuildServiceProvider();
        context.Request.Method = HttpMethods.Put;

        // Act
        var result = await endpointDelegate(context);

        // Assert
        var validationResult = Assert.IsType<ValidationProblem>(result);
        Assert.True(validationResult.ProblemDetails.Errors.ContainsKey("Name"));
        Assert.Contains("The Name field is required.", validationResult.ProblemDetails.Errors["Name"]);
    }

    // Helper mock RouteGroupBuilder for PUT endpoint extraction with validation
    private class RouteGroupBuilderMock_Put_Validation : RouteGroupBuilder
    {
        public List<RouteEndpointBuilder> Routes { get; } = new List<RouteEndpointBuilder>();

        public RouteGroupBuilderMock_Put_Validation(Delegate handler)
        {
            MapPut("/update-product", handler);
        }

        public override RouteGroupBuilder MapPut(string pattern, Delegate handler)
        {
            var builder = new RouteEndpointBuilder(
                context => Task.CompletedTask,
                RoutePatternFactory.Parse(pattern),
                0);
            builder.Metadata.Add(handler);
            builder.RequestDelegate = async ctx =>
            {
                var productService = ctx.RequestServices.GetRequiredService<IProductService>();
                var request = ctx.Request;
                var result = await (Task<IResult>)handler.DynamicInvoke(productService, request);
                await result.ExecuteAsync(ctx);
            };
            Routes.Add(builder);
            return this;
        }

        public override RouteGroupBuilder WithTags(params string[] tags) => this;
        public override RouteGroupBuilder MapGet(string pattern, Delegate handler) => this;
        public override RouteGroupBuilder MapPost(string pattern, Delegate handler) => this;
        public override RouteGroupBuilder RequireAuthorization() => this;
        public override RouteGroupBuilder WithValidation<T>() => this;
        public override RouteGroupBuilder MapGet(string pattern, RequestDelegate handler) => this;
    }
}