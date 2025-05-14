using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UserService.API.Application.Shared.Constant;

namespace UserService.API.Infrastructure.Database;
public class LmsDbContextFactory : IDesignTimeDbContextFactory<CampingDbContext>
{
    public CampingDbContext CreateDbContext(string[] args)
    {
        string envPath = Path.GetFullPath(Path.Combine
            (AppDomain.CurrentDomain.BaseDirectory, "../../../../../../lms_server/.env"));
        Env.Load(envPath);

        var optionsBuilder = new DbContextOptionsBuilder<CampingDbContext>();
        optionsBuilder.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));
        
        return new CampingDbContext(optionsBuilder.Options);
    }
}
