using AuthenticationService.API.Application.Bindings;
using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.DTOs.OTP;
using AuthenticationService.API.Application.Services;
using AuthenticationService.API.Application.Validators;

namespace AuthenticationService.API.Application.Endpoints;
public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this RouteGroupBuilder group)
    {
        var authGroup = group.WithTags("Authentication");

        authGroup.MapPost("/login", async (IAuthenticationService authenticationService, LoginRequest request) =>
        {
            var result = await authenticationService.Login(request);
            return Results.Ok(result);
        })
        .WithName("Login")
        .WithValidation<LoginRequest>();

        authGroup.MapPost("/register", async (IAuthenticationService authenticationService, RegisterRequest request) =>
        {
            var result = await authenticationService.Register(request);
            return Results.Ok(result);
        })
        .WithName("Register")
        .WithValidation<RegisterRequest>();

        authGroup.MapPost("/refresh-token", async (IAuthenticationService authenticationService, RefreshTokenRequest request) =>
        {
            var result = await authenticationService.RefreshToken(request);
            return Results.Ok(result);
        })
        .WithName("Refresh Token")
        .WithValidation<RefreshTokenRequest>();

        authGroup.MapDelete("/logout", async (IAuthenticationService authenticationService, UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var result = await authenticationService.LogOut(userId.Value);
            return Results.Ok(result);
        })
        .WithName("Logout")
        .RequireAuthorization();

        authGroup.MapPost("/verify-email", async (IAuthenticationService authenticationService, RegisterVerifyRequest request) =>
        {
            var result = await authenticationService.VerifyEmail(request);
            return Results.Ok(result);
        })
        .WithName("Verify Email")
        .WithValidation<RegisterVerifyRequest>();

        authGroup.MapPost("/verify-otp", async (IAuthenticationService authenticationService, VerifyOtpRequest request) =>
        {
            var result = await authenticationService.VerifyOtp(request);
            return Results.Ok(result);
        })
        .WithName("Verify OTP")
        .WithValidation<VerifyOtpRequest>();

        return authGroup;
    }
}