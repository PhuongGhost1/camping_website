using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Application.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            return await _authenticationService.Login(req);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            return await _authenticationService.Register(req);
        }
    }
}
