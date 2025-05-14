using Microsoft.AspNetCore.Mvc;
using UserService.API.Application.Shared.Type;
using UserService.API.Domain;
using UserService.API.Infrastructure.Repository;

namespace UserService.API.Application.Services;

public interface IUserServices
{
    Task<IActionResult> GetUserInfo(Guid? userId);
}

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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
}
