using AuthenticationService.Grpc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthenticationService.API.Core.Helper;

namespace AuthenticationService.API.Application.Grpc.Clients
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthGrpcClient _authGrpcClient;
        public AuthController(AuthGrpcClient authGrpcClient)
        {
            _authGrpcClient = authGrpcClient;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginGrpcRequest request)
        {
            var response = await _authGrpcClient.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterGrpcRequest request)
        {
            var response = await _authGrpcClient.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenGrpcRequest request)
        {
            var response = await _authGrpcClient.RefreshTokenAsync(request);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            var metadata = Payload.GetAuthorizationHeaders(Request);
            var response = await _authGrpcClient.LogoutAsync(new LogoutGrpcRequest(), metadata);
            return Ok(response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] RegisterVerifyGrpcRequest request)
        {
            var response = await _authGrpcClient.VerifyEmailAsync(request);
            return Ok(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpGrpcRequest request)
        {
            var response = await _authGrpcClient.VerifyOtpAsync(request);
            return Ok(response);
        }
    }
}