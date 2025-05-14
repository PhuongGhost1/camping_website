namespace OrderService.API.Application.Shared.Constant;
public static class RabbitMQConst
{
    public static string RABBITMQ_HOST = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ??
        throw new Exception("RABBITMQ_HOST is not set");
    public static string RABBITMQ_PORT = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ??
        throw new Exception("RABBITMQ_PORT is not set");
    public static string RABBITMQ_USERNAME = Environment.GetEnvironmentVariable("RABBITMQ_USER") ??
        throw new Exception("RABBITMQ_USERNAME is not set");    
    public static string RABBITMQ_PASSWORD = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ??
        throw new Exception("RABBITMQ_PASSWORD is not set");
}