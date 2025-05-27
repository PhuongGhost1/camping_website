using AuthenticationService.API.Application.Validators;
using AuthenticationService.API.Core.Helper;
using AuthenticationService.Grpc;

namespace AuthenticationService.API.Application.Grpc.Clients;
public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this RouteGroupBuilder group)
    {
        var authGroup = group.WithTags("Authentication");

        authGroup.MapPost("/login", async (AuthGrpcClient _client, LoginGrpcRequest request) =>
        {
            var result = await _client.LoginAsync(request);
            return Results.Ok(result);
        })
        .WithName("Login")
        .WithValidation<LoginGrpcRequest>();

        authGroup.MapPost("/register", async (AuthGrpcClient _client, RegisterGrpcRequest request) =>
        {
            var result = await _client.RegisterAsync(request);
            return Results.Ok(result);
        })
        .WithName("Register")
        .WithValidation<RegisterGrpcRequest>();

        authGroup.MapPost("/refresh-token", async (AuthGrpcClient _client, RefreshTokenGrpcRequest request) =>
        {
            var result = await _client.RefreshTokenAsync(request);
            return Results.Ok(result);
        })
        .WithName("Refresh Token")
        .WithValidation<RefreshTokenGrpcRequest>();

        authGroup.MapDelete("/logout", async (AuthGrpcClient _client, HttpContext httpContext) =>
        {
            var metadata = Payload.GetAuthorizationHeaders(httpContext.Request);
            var result = await _client.LogoutAsync(new LogoutGrpcRequest(), metadata);
            return Results.Ok(result);
        })
        .WithName("Logout")
        .RequireAuthorization();

        authGroup.MapPost("/verify-email", async (AuthGrpcClient _client, RegisterVerifyGrpcRequest request) =>
        {
            var result = await _client.VerifyEmailAsync(request);
            return Results.Ok(result);
        })
        .WithName("Verify Email")
        .WithValidation<RegisterVerifyGrpcRequest>();

        authGroup.MapPost("/verify-otp", async (AuthGrpcClient _client, VerifyOtpGrpcRequest request) =>
        {
            var result = await _client.VerifyOtpAsync(request);
            return Results.Ok(result);
        })
        .WithName("Verify OTP")
        .WithValidation<VerifyOtpGrpcRequest>();

        return authGroup;
    }
}