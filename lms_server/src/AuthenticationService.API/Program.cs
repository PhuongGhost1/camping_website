using AuthenticationService.API.Infrastructure.Database;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using AuthenticationService.API.Application.Shared.Constant;
using AuthenticationService.API.Infrastructure.Repository.Authentication;
using AuthenticationService.API.Application.Services;
using AuthenticationService.API.Core.Jwt;
using System.Text.Json.Serialization;

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CampingDbContext>(options =>
{
    Console.WriteLine($"Using ConnectionString: {MySQLDatabase.DB_CONNECTION_STRING}");
    options.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));

});

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationServices>();

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

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
