using OrderService.API.Application.Shared.Constant;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OrderService.API.Infrastructure.Database;
public class LmsDbContextFactory : IDesignTimeDbContextFactory<CampingDbContext>
{
    public CampingDbContext CreateDbContext(string[] args)
    {
        string envPath = Path.GetFullPath(Path.Combine
            (AppDomain.CurrentDomain.BaseDirectory, "../../../../../../lms_server/.env"));
        Env.Load(envPath);

        var optionsBuilder = new DbContextOptionsBuilder<CampingDbContext>();

        var connectionString = MySQLDatabase.DB_CONNECTION_STRING;

        optionsBuilder.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));
        
        return new CampingDbContext(optionsBuilder.Options);
    }
}
