namespace ProductService.API.Application.Shared.Constant;
public static class RedisConstant
{
    public static string REDIS_HOST = Environment.GetEnvironmentVariable("REDIS_HOST") ?? 
    throw new ArgumentNullException("Cannot find redis host","REDIS_HOST");
    public static string REDIS_PORT = Environment.GetEnvironmentVariable("REDIS_PORT") ??
    throw new ArgumentNullException("Cannot find redis port","REDIS_PORT");
    public static string REDIS_CONNECTION_STRING = 
    $"{REDIS_HOST}:{REDIS_PORT},abortConnect=false,connectTimeout=5000,keepAlive=18000,syncTimeout=5000,defaultDatabase=0";
}