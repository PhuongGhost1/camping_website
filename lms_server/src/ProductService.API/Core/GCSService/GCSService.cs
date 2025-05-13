using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using ProductService.API.Application.Shared.Constant;

namespace ProductService.API.Core.GCSService;

public interface IGCSService
{
    Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType);
}

// public class GCSService : IGCSService
// {
//     private readonly string PROJECT_ID = Firebase.FIREBASE_PROJECT_ID;
//     private const string FOLDER = "media/";
//     private readonly string BucketName;
//     private readonly StorageClient storageClent;
//     private readonly string FilePrefix;

//     public GCSService()
//     {
//         // Initialize the Google Cloud Storage client
//         string credentialsPath = "../../firebase_security.json";

//         var credentials = GoogleCredential.FromFile(credentialsPath);
//         storageClent = StorageClient.Create(credentials);
//         BucketName = PROJECT_ID + ".appspot.com"; // Default Firebase Storage bucket
//         FilePrefix = "https://storage.googleapis.com/" + BucketName + "/";
//     }

//     public async Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType)
//     {
//         try
//         {
//             var objectName = FOLDER + destinationFileName;

//             // Upload the file to Firebase Storage
//             var options = new UploadObjectOptions
//             {
//                 PredefinedAcl = PredefinedObjectAcl.PublicRead // Set the PredefinedAcl to PublicRead
//             };

//             await storageClent.UploadObjectAsync(BucketName, objectName, contentType, fileStream, options);

//             // Generate a URL to the uploaded content
//             var fileUrl = FilePrefix + objectName;
//             return fileUrl;
//         }
//         catch (Exception ex)
//         {
//             // Handle any exceptions that may occur during the upload
//             Console.WriteLine($"Error uploading file: {ex.Message}");
//             return null;
//         }
//     }
// }

public class LocalStorageService : IGCSService
{
    private readonly string _folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
    private readonly string _urlPrefix = "/uploads/";

    public LocalStorageService()
    {
        if (!Directory.Exists(_folder))
        {
            Directory.CreateDirectory(_folder);
        }
    }

    public async Task<string?> UploadFileAsync(Stream fileStream, string destinationFileName, string contentType)
    {
        try
        {
            var filePath = Path.Combine(_folder, destinationFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            var fileUrl = _urlPrefix + destinationFileName;
            return fileUrl; // Return the URL to access the file
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file locally: {ex.Message}");
            return null;
        }
    }
}