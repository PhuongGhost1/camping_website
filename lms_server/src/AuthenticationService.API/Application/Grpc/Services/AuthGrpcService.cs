using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.Grpc.Extensions;
using AuthenticationService.API.Application.Services;
using AuthenticationService.Grpc;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Application.Grpc.Services;

public class AuthGrpcService : AuthService.AuthServiceBase
{
    private readonly IAuthenticationService _authenticationService;
    public AuthGrpcService(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    public override async Task<LoginGrpcResponse> Login(LoginGrpcRequest request, ServerCallContext context)
    {
        var result = await _authenticationService.Login(request.ToLoginRequest()) as JsonResult;
        var data = result?.Value as LoginResponse;

        return data?.ToLoginResponse() ?? new LoginGrpcResponse();
    }

    public override async Task<RegisterGrpcResponse> Register(RegisterGrpcRequest request, ServerCallContext context)
    {
        var result = await _authenticationService.Register(request.ToRegisterRequest()) as JsonResult;
        var data = result?.Value as RegisterResponse;

        return data?.ToRegisterResponse() ?? new RegisterGrpcResponse();
    }

    public override async Task<RefreshTokenGrpcResponse> RefreshToken(RefreshTokenGrpcRequest request, ServerCallContext context)
    {
        var result = await _authenticationService.RefreshToken(request.ToRefreshTokenRequest()) as JsonResult;
        var data = result?.Value as RefreshTokenResponse;

        return data?.ToRefreshTokenResponse() ?? new RefreshTokenGrpcResponse();
    }

    [Authorize]
    public override async Task<LogoutGrpcResponse> Logout(LogoutGrpcRequest request, ServerCallContext context)
    {
        var userId = AuthGrpcMapper.GetUserIdFromClaims(context);
        if (userId == Guid.Empty) return AuthGrpcMapper.ToLogoutResponse("User not authorized!");

        var result = await _authenticationService.LogOut(userId) as JsonResult;
        var message = AuthGrpcMapper.ExtractMessageFromResult(result);

        return AuthGrpcMapper.ToLogoutResponse(message ?? "Logout failed!");
    }

    public override async Task<GenericGrpcResponse> VerifyEmail(RegisterVerifyGrpcRequest request, ServerCallContext context)
    {
        var result = await _authenticationService.VerifyEmail(request.ToRegisterVerifyRequest()) as JsonResult;
        var message = AuthGrpcMapper.ExtractMessageFromResult(result);

        return AuthGrpcMapper.ToGenericGrpcResponse(message ?? "Email verification failed!");
    }
    
    public override async Task<RegisterGrpcResponse> VerifyOtp(VerifyOtpGrpcRequest request, ServerCallContext context)
    {
        var result = await _authenticationService.VerifyOtp(request.ToVerifyOtpRequest()) as JsonResult;
        var data = result?.Value as RegisterResponse;

        return data?.ToRegisterResponse() ?? new RegisterGrpcResponse();
    }
}