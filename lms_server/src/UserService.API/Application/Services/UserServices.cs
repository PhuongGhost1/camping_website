using Microsoft.AspNetCore.Mvc;
using UserService.API.Application.DTOs.User;
using UserService.API.Application.Shared.Type;
using UserService.API.Core.Jwt;
using UserService.API.Infrastructure.Repository;

namespace UserService.API.Application.Services;

public interface IUserServices
{
    Task<IActionResult> GetUserInfo(Guid? userId);
    Task<IActionResult> UpdateUserInfo(Guid userId, UpdateUserInfoReq req);
    Task<IActionResult> UpdateUserPwd(Guid userId, UpdateUserPwdReq req);
}

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    public UserServices(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<IActionResult> GetUserInfo(Guid? userId)
    {
        try
        {
            if(userId == Guid.Empty || userId == null)
                return ErrorResp.Unauthorized("User ID is empty or null");

            var user = await _userRepository.GetUserInfo(userId);
            if (user == null)
                return ErrorResp.NotFound("User not found");

            return SuccessResp.Ok(user);
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> UpdateUserInfo(Guid userId, UpdateUserInfoReq req)
    {
        try
        {
            var user = await _userRepository.GetUserInfo(userId);
            if (user == null)
                return ErrorResp.NotFound("User not found");

            user.Name = req.FirstName + " " + req.LastName;

            var isUpdated = await _userRepository.UpdateUserInfo(user);
            if (!isUpdated)
                return ErrorResp.InternalServerError("Failed to update user information");

            return SuccessResp.Ok(new UpdateUserResp
            {
                Status = 200,
                Message = "User information updated successfully"
            });    
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> UpdateUserPwd(Guid userId, UpdateUserPwdReq req)
    {
        try
        {
            var user = await _userRepository.GetUserInfo(userId);
            if (user == null)
                return ErrorResp.NotFound("User not found");

            if (string.IsNullOrEmpty(user?.PasswordHash))
                return ErrorResp.BadRequest("Current password is missing for the user");

            var oldPwd = _jwtService.HashObject<string>(user.PasswordHash);

            if (req.NewPassword == oldPwd)
                return ErrorResp.BadRequest("New password cannot be the same as the old password");

            if (req.NewPassword != req.ConfirmPassword)
                return ErrorResp.BadRequest("New password and confirm password do not match");

            user.PasswordHash = _jwtService.HashObject<string>(req.NewPassword);

            var isUpdated = await _userRepository.UpdateUserInfo(user);
            if (!isUpdated)
                return ErrorResp.InternalServerError("Failed to update user password");

            return SuccessResp.Ok(new UpdateUserResp
            {
                Status = 200,
                Message = "User password updated successfully"
            });
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
