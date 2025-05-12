using AuthenticationService.API.Application.Shared.Constant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthenticationService.API.Infrastructure.Database;
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
