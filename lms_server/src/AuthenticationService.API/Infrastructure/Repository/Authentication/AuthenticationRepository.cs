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
}
