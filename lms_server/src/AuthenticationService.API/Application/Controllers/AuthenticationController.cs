using System.Security.Claims;
using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Application.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            _logger.LogInformation("Login Request");
            return await _authenticationService.Login(req);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            _logger.LogInformation("Register Request");
            return await _authenticationService.Register(req);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest req)
        {
            _logger.LogInformation("Refresh Token Request");
            return await _authenticationService.RefreshToken(req);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut()
        {
            _logger.LogInformation("Logout Request");
            var userId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return await _authenticationService.LogOut(userId);
        }
    }
}
