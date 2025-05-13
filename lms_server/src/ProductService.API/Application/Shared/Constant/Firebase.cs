using DotNetEnv;

namespace ProductService.API.Application.Shared.Constant;
public static class Firebase
{
    static Firebase()
    {
        string envPath = Path.GetFullPath(Path.Combine
            (AppDomain.CurrentDomain.BaseDirectory, "../../../../../../lms_server/.env"));
        Env.Load(envPath);
    }
    public static readonly string FIREBASE_PROJECT_ID =
        Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID")
        ?? throw new EnvVariableNotFoundException("Firebase key not found in environment variables", "FIREBASE_BUCKET");
}
