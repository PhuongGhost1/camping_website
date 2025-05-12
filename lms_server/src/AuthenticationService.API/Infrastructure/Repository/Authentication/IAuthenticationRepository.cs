using AuthenticationService.API.Domain;

namespace AuthenticationService.API.Infrastructure.Repository.Authentication;
public interface IAuthenticationRepository
{
    Task<bool> IsEmailExists(string email);
    Task<Users?> Login(string email, string pwd);
    Task<bool> Register(Users users);
}
