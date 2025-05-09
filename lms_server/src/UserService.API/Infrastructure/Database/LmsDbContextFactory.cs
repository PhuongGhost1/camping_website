using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UserService.API.Application.Shared.Type;

namespace UserService.API.Infrastructure.Database;
public class LmsDbContextFactory : IDesignTimeDbContextFactory<CampingDbContext>
{
    public CampingDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"Using ConnectionString: {MySQLDatabase.DB_CONNECTION_STRING}");

        var optionsBuilder = new DbContextOptionsBuilder<CampingDbContext>();
        optionsBuilder.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));
        
        return new CampingDbContext(optionsBuilder.Options);
    }
}
