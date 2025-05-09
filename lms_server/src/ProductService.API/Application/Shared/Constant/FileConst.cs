namespace ProductService.API.Application.Shared.Constant;
public static class FileConst
{
    public const string IMAGE = "image";

    public const int MAX_IMAGE_SIZE = 5 * 1024 * 1024; // 5MB

    public static readonly string[] IMAGE_CONTENT_TYPES = { "image/jpeg", "image/png", "image/gif", "image/webp" };
}
