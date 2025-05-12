using AuthenticationService.API.Domain;

namespace AuthenticationService.API.Application.DTOs.Authentication;

public record LoginRequest(string email, string pwd);
public record LoginResponse(string email, string pwd, string result);

public record RegisterRequest(string name, string email, string pwd);
public record RegisterResponse(Users user, string result);
