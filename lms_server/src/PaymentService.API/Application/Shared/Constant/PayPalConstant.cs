using DotNetEnv;

namespace PaymentService.API.Application.Shared.Constant;

public static class PayPalConstant
{
    static PayPalConstant()
    {
        string envPath = Path.GetFullPath(Path.Combine
            (AppDomain.CurrentDomain.BaseDirectory, "../../../../../../lms_server/.env"));
        Env.Load(envPath);
    }
    public static string CLIENT_ID = Environment.GetEnvironmentVariable("PAYPAL_CLIENT_ID") ??
        throw new Exception("PayPal Client ID is not set in the environment variables.");
    public static string CLIENT_SECRET = Environment.GetEnvironmentVariable("PAYPAL_CLIENT_SECRET") ??
        throw new Exception("PayPal Client Secret is not set in the environment variables.");
    public static string MODE = Environment.GetEnvironmentVariable("PAYPAL_MODE") ??
        throw new Exception("PayPal Mode is not set in the environment variables.");    
}