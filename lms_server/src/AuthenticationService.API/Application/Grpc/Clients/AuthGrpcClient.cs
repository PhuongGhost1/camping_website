using AuthenticationService.Grpc;
using Grpc.Core;

namespace AuthenticationService.API.Application.Grpc.Clients;

public class AuthGrpcClient
{
    private readonly AuthService.AuthServiceClient _client;
    public AuthGrpcClient(AuthService.AuthServiceClient client)
    {
        _client = client;
    }

    public async Task<LoginGrpcResponse> LoginAsync(LoginGrpcRequest request)
    {
        return await _client.LoginAsync(request);
    }

    public async Task<RegisterGrpcResponse> RegisterAsync(RegisterGrpcRequest request)
    {
        return await _client.RegisterAsync(request);
    }

    public async Task<RefreshTokenGrpcResponse> RefreshTokenAsync(RefreshTokenGrpcRequest request)
    {
        return await _client.RefreshTokenAsync(request);
    }

    public async Task<LogoutGrpcResponse> LogoutAsync(LogoutGrpcRequest request, Metadata? headers = null)
    {
        return await _client.LogoutAsync(request, headers);
    }

    public async Task<GenericGrpcResponse> VerifyEmailAsync(RegisterVerifyGrpcRequest request)
    {
        return await _client.VerifyEmailAsync(request);
    }
    
    public async Task<RegisterGrpcResponse> VerifyOtpAsync(VerifyOtpGrpcRequest request)
    {
        return await _client.VerifyOtpAsync(request);
    }
}