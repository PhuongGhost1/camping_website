using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.API.Application.Services;

namespace UserService.API.Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IAuthServices _authServices;
        public UserController(IUserServices userServices, IAuthServices authServices)
        {
            _userServices = userServices;
            _authServices = authServices;
        }
    }
}
