namespace Gateway.API.Application.Constant;
public static class JwtConst
{
    public const int ACCESS_TOKEN_EXP = 15 * 60; // 15m
    public const int REFRESH_TOKEN_EXP = 3600 * 24 * 30; // 30 days
    public static string JWT_SECRET_KEY = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ??
        throw new ApplicationException("Cannot found jwt key in environment variables");
}