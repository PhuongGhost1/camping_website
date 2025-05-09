using Microsoft.EntityFrameworkCore;

namespace UserService.API.Infrastructure.Database;
public static class DbInitializer
{
    public static IApplicationBuilder UseInitializeDatabase(this IApplicationBuilder application)
    {
        using var serviceScope = application.ApplicationServices.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<CampingDbContext>();

        if(dbContext != null && dbContext.Database.GetPendingMigrations().Any())
        {
            Console.WriteLine("Applying  Migrations...");
            dbContext.Database.Migrate();
        }
        throw new Exception("Pending Migrations");
    }
}
