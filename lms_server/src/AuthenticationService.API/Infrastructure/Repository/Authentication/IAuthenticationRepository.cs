using AuthenticationService.API.Domain;

namespace AuthenticationService.API.Infrastructure.Repository.Authentication;
public interface IAuthenticationRepository
{
    Task<bool> IsEmailExists(string email);
    Task<Users?> Login(string email, string pwd);
    Task<bool> Register(Users users);
    Task<Refreshtokens?> GetRefreshToken(string refreshToken);
    Task<bool> SaveRefreshToken(Refreshtokens refreshToken);
    Task<bool> UpdateRefreshToken(Refreshtokens refreshTokens);
    Task<bool> DeleteRefreshTokenByUserId(Guid userId);
    Task<Users?> GetUserById(Guid? userId);
}
