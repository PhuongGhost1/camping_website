using UserService.API.Domain;

namespace UserService.API.Infrastructure.Repository;
public interface IUserRepository
{
    Task<Users?> GetUserInfo(Guid? userId);
}
