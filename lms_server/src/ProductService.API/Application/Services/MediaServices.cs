using ProductService.API.Application.Shared.Constant;
using ProductService.API.Application.Shared.Enum;
using ProductService.API.Core.GCSService;

namespace ProductService.API.Application.Services;

public interface IMediaServices
{
    Task<string?> HandleUploadMedia(IFormFile file, MediaTypeEnum type);
    Task<string?> HandleCreateMedia(IFormFile file, MediaTypeEnum type);
}
public class MediaServices : IMediaServices
{
    private readonly IGCSService _gcsService;

    public MediaServices(IGCSService gcsService)
    {
        _gcsService = gcsService;
    }

    public async Task<string?> HandleCreateMedia(IFormFile file, MediaTypeEnum type)
    {
        if (file == null || file.Length == 0)
        {
            return "File is empty";
        }

        var contentType = file.ContentType;

        if (type == MediaTypeEnum.Image)
        {
            if (!FileConst.IMAGE_CONTENT_TYPES.Contains(contentType))
            {
                return "File is not image type (jpg, png, gif, webp)";
            }
            else if (file.Length > FileConst.MAX_IMAGE_SIZE)
            {
                return "File size is too large";
            }
        }else{
            return "File type is not supported";
        }

        var downloadUrl = await ProcessUpload(file, contentType);
        if (downloadUrl is null)
        {
            return "Failed to upload file";
        }

        return downloadUrl;
    }

    public async Task<string?> HandleUploadMedia(IFormFile file, MediaTypeEnum type)
    {
        if (file == null || file.Length == 0)
        {
            return "File is empty";
        }

        var contentType = file.ContentType;

        if (type == MediaTypeEnum.Image)
        {
            if (!FileConst.IMAGE_CONTENT_TYPES.Contains(contentType))
            {
                return "File is not image type (jpg, png, gif, webp)";
            }
            else if (file.Length > FileConst.MAX_IMAGE_SIZE)
            {
                return "File size is too large";
            }
        }else{
            return "File type is not supported";
        }

        var downloadUrl = await ProcessUpload(file, contentType);
        if (downloadUrl is null)
        {
            return "Failed to upload file";
        }

        return downloadUrl;
    }

    public async Task<string?> ProcessUpload(IFormFile file, string contentType)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // upload file to GCS
        var fileStream = file.OpenReadStream();

        var downloadUrl = await _gcsService.UploadFileAsync(fileStream, fileName, contentType);

        if (downloadUrl is null) return null;

        return downloadUrl;
    }
}