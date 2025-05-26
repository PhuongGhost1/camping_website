using Grpc.Core;

namespace AuthenticationService.API.Core.Helper;
public static class Payload
{
    public static Metadata GetAuthorizationHeaders(HttpRequest request)
    {
        var token = request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        var metadata = new Metadata();
        if (!string.IsNullOrEmpty(token))
        {
            metadata.Add("Authorization", $"Bearer {token}");
        }
        return metadata;
    }
}