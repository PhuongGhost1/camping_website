using AuthenticationService.API.Domain;
using AuthenticationService.API.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.API.Infrastructure.Repository.Authentication;
public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly CampingDbContext _context;
    public AuthenticationRepository(CampingDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteRefreshTokenByUserId(Guid userId)
    {
        return await _context.Refreshtokens
            .Where(r => r.UserId.Equals(userId))
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<Refreshtokens?> GetRefreshToken(string refreshToken)
    {
        return await _context.Refreshtokens
            .Where(r => r.Token.ToLower().Equals(refreshToken.ToLower()))
            .FirstOrDefaultAsync();
    }

    public Task<Users?> GetUserById(Guid? userId)
    {
        return _context.Users
            .Where(u => u.Id.Equals(userId))
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsEmailExists(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));
    }

    public async Task<Users?> Login(string email, string pwd)
    {
        return await _context.Users.Where(u => u.Email.ToLower().Equals(email.ToLower()) && 
                                    u.PasswordHash.ToLower().Equals(pwd.ToLower())).FirstOrDefaultAsync();
    }

    public async Task<bool> Register(Users users)
    {
        await _context.Users.AddAsync(users);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> SaveRefreshToken(Refreshtokens refreshToken)
    {
        await _context.Refreshtokens.AddAsync(refreshToken);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateRefreshToken(Refreshtokens refreshToken)
    {
        _context.Refreshtokens.Update(refreshToken);
        return await _context.SaveChangesAsync() > 0;
    }
}
