using DotNetEnv;

namespace ProductService.API.Application.Shared.Constant;
public static class Firebase
{
    public const string FIREBASE_BUCKET = Environment.GetEnvironmentVariable("FIREBASE_BUCKET") ?? 
        throw new EnvVariableNotFoundException("Firebase key not found in environment variables", "FIREBASE_BUCKET");
}
