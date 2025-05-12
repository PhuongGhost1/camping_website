using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.Shared.Constant;
using AuthenticationService.API.Application.Shared.Constant.Type;
using AuthenticationService.API.Core.Jwt;
using AuthenticationService.API.Domain;
using AuthenticationService.API.Infrastructure.Repository.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Application.Services;

public interface IAuthenticationService
{
    Task<IActionResult> Login(LoginRequest req);
    Task<IActionResult> Register(RegisterRequest req);
}
public class AuthenticationServices : IAuthenticationService
{
    private readonly IAuthenticationRepository _authRepo;
    private readonly IJwtService _jwtService;
    public AuthenticationServices(IAuthenticationRepository authRepo, IJwtService jwtService)
    {
        _authRepo = authRepo;
        _jwtService = jwtService;
    }
    public async Task<IActionResult> Login(LoginRequest req)
    {
        try
        {
            var user = await _authRepo.Login(req.email, _jwtService.HashObject<string>(req.pwd));

            if (user is null) return ErrorResp.Unauthorized("Login Failed!");

            return SuccessResp.Ok(new LoginResponse
            (
                user.Email,
                user.PasswordHash,
                _jwtService.GenerateToken(user.Id, Guid.NewGuid(), user.Email, JwtConst.ACCESS_TOKEN_EXP)
            ));
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IActionResult> Register(RegisterRequest req)
    {
        try
        {
            var isExist = await _authRepo.IsEmailExists(req.email);

            if (isExist) return ErrorResp.BadRequest("Email already exist!");

            var newUser = new Users
            {
                Name = req.name,
                Email = req.email,
                PasswordHash = _jwtService.HashObject<string>(req.pwd)
            };

            var user = await _authRepo.Register(newUser);

            return user ? SuccessResp.Ok(new RegisterResponse(newUser, "Register Successfully!")) 
            : ErrorResp.BadRequest("Faild to create user!");
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
