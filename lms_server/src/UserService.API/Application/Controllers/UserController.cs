using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Application.DTOs.User;
using UserService.API.Application.Services;

namespace UserService.API.Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Authorize]
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userId = Guid.Parse(User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value);
            return await _userServices.GetUserInfo(userId);
        }

        [Authorize]
        [HttpPut("update-info")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoReq req)
        {
            var userId = Guid.Parse(User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value);
            return await _userServices.UpdateUserInfo(userId, req);
        }

        [Authorize]
        [HttpPut("update-pwd")]
        public async Task<IActionResult> UpdateUserPwd([FromBody] UpdateUserPwdReq req)
        {
            var userId = Guid.Parse(User.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value);
            return await _userServices.UpdateUserPwd(userId, req);
        }
    }
}
