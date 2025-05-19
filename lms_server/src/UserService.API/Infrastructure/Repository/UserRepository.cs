using Microsoft.EntityFrameworkCore;
using UserService.API.Domain;
using UserService.API.Infrastructure.Database;

namespace UserService.API.Infrastructure.Repository;
public class UserRepository : IUserRepository
{
    private readonly CampingDbContext _dbContext;
    public UserRepository(CampingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Users?> GetUserInfo(Guid? userId)
    {
        return await _dbContext.Users
            .Include(u => u.Reviews)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> UpdateUserInfo(Users user)
    {
        _dbContext.Users.Update(user);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}
