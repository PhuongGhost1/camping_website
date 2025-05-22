namespace ProductService.API.Application.DTOs.Product;

public static class ParseProductReq
{
    public static async Task<CreateProductReq> ParseCreateProductReq(HttpRequest request)
    {
        var form = await request.ReadFormAsync();
        return new CreateProductReq(
            form["name"]!,
            form["description"]!,
            decimal.Parse(form["price"]!),
            int.Parse(form["stock"]!),
            form.Files.GetFile("imageUrl")!,
            Guid.Parse(form["categoryId"]!)
        );
    }
    
    public static async Task<UpdateProductReq> ParseUpdateProductReq(HttpRequest request)
    {
        var form = await request.ReadFormAsync();
        return new UpdateProductReq(
            Guid.Parse(form["id"]!),
            form["name"],
            form["description"],
            decimal.Parse(form["price"]),
            int.Parse(form["stock"]),
            form.Files.GetFile("imageUrl"),
            Guid.Parse(form["categoryId"]) 
        );
    }
}