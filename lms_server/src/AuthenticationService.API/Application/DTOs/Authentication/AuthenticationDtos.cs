using AuthenticationService.API.Domain;

namespace AuthenticationService.API.Application.DTOs.Authentication;

public record LoginRequest(string email, string pwd);
public record LoginResponse(string email, string pwd, string accessToken, string refreshToken);

public record RegisterRequest(string name, string email, string pwd);
public record RegisterResponse(Users user, string result);

public record RefreshTokenRequest(string refreshToken);
public record RefreshTokenResponse(string accessToken, string refreshToken);

public record RegisterVerifyRequest(string name, string email, string pwd);
public record RegisterVerifyResponse(string name, string email, string pwd, string otp);