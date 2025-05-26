using System.Security.Claims;
using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.DTOs.OTP;
using AuthenticationService.Grpc;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using ProtoUsers = AuthenticationService.Grpc.Users;

namespace AuthenticationService.API.Application.Grpc.Extensions;

public static class AuthGrpcMapper
{
    public static LoginRequest ToLoginRequest(this LoginGrpcRequest req)
    => new LoginRequest
    (
        req.Email,
        req.Password
    );

    public static LoginGrpcResponse ToLoginResponse(this LoginResponse res)
    => new LoginGrpcResponse
    {
        AccessToken = res.accessToken,
        RefreshToken = res.refreshToken,
        Email = res.email,
        Password = res.pwd,
    };

    public static RegisterRequest ToRegisterRequest(this RegisterGrpcRequest req)
    => new RegisterRequest
    (
        req.Email,
        req.Password,
        req.FullName
    );

    public static RegisterGrpcResponse ToRegisterResponse(this RegisterResponse res)
    => new RegisterGrpcResponse
    {
        User = new ProtoUsers
        {
            Email = res.user.Email,
            Name = res.user.Name,
            AvatarUrl = res.user.AvatarUrl
        },
        Message = res.result
    };

    public static RefreshTokenRequest ToRefreshTokenRequest(this RefreshTokenGrpcRequest req)
    => new RefreshTokenRequest
    (
        req.RefreshToken
    );

    public static RefreshTokenGrpcResponse ToRefreshTokenResponse(this RefreshTokenResponse res)
    => new RefreshTokenGrpcResponse
    {
        AccessToken = res.accessToken,
        RefreshToken = res.refreshToken
    };

    public static Guid GetUserIdFromClaims(ServerCallContext context)
    {
        var httpContext = context.GetHttpContext();
        var user = httpContext.User;

        var claim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");

        return Guid.TryParse(claim?.Value, out var userId) ? userId : Guid.Empty;
    }

    public static LogoutGrpcResponse ToLogoutResponse(this string message)
    => new LogoutGrpcResponse { Message = message };
    

    public static string? ExtractMessageFromResult(JsonResult? result)
    {
        if (result?.Value == null) return null;

        var prop = result.Value.GetType().GetProperty("Message");
        return prop?.GetValue(result.Value)?.ToString();
    }

    public static RegisterVerifyRequest ToRegisterVerifyRequest(this RegisterVerifyGrpcRequest req)
    => new RegisterVerifyRequest
    (
        req.Name,
        req.Email,
        req.Password
    );

    public static VerifyOtpRequest ToVerifyOtpRequest(this VerifyOtpGrpcRequest req)
    => new VerifyOtpRequest
    {
        Email = req.Email,
        Otp = req.OtpCode
    };

    public static GenericGrpcResponse ToGenericGrpcResponse(this string res)
    => new GenericGrpcResponse
    {
        Message = res
    };
}