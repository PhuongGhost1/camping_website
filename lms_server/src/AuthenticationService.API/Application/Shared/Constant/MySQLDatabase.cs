namespace AuthenticationService.API.Application.Shared.Constant;
public static class MySQLDatabase
{
    public static string DB_CONNECTION_STRING = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? 
        throw new ApplicationException("Connection string not found in environment variables.");
}
