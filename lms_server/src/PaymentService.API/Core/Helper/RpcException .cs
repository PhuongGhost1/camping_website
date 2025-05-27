using System.Text.Json;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;

namespace PaymentService.API.Core.Helper;
public static class GrpcHelper
{
    public static string ExtractMessageFromResultHelper(this JsonResult result)
    {
        if (result.Value is not null)
        {
            var dict = result.Value as Dictionary<string, object>;
            if (dict != null)
            {
                if (dict.TryGetValue("Message", out var msg))
                    return msg?.ToString() ?? "No message provided";

                if (dict.TryGetValue("Error", out var err))
                    return err?.ToString() ?? "No error message provided";
            }
        }

        return "No response message";
    }

    public static void ThrowIfFailed(this IActionResult result)
    {
        switch (result)
        {
            case JsonResult json:
                var statusCode = json.StatusCode ?? 500;
                if (statusCode >= 400)
                {
                    var message = json.ExtractMessageFromResultHelper();
                    throw new RpcException(new Status(StatusCode.Internal, message));
                }
                break;
            default:
                throw new RpcException(new Status(StatusCode.Unknown, "Unexpected result type from internal service"));
        }
    }
}