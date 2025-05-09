using DotNetEnv;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using UserService.API.Application.Services;
using UserService.API.Application.Shared.Type;
using UserService.API.Infrastructure.Database;
using UserService.API.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

string envPath = Path.GetFullPath(Path.Combine
            (AppDomain.CurrentDomain.BaseDirectory, "../../../../../../lms_server/.env"));
Env.Load(envPath);

// Add services to the container.
builder.Services.AddRateLimiter(_ => _
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    }));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
      builder =>
      {
          builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
      });
    options.AddDefaultPolicy(
      builder =>
      {
          builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CampingDbContext>(options =>
{
    Console.WriteLine($"Using ConnectionString: {MySQLDatabase.DB_CONNECTION_STRING}");
    options.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));

});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();

var app = builder.Build();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/swagger/index.html");
    await Task.CompletedTask;
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAllOrigins");
app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
