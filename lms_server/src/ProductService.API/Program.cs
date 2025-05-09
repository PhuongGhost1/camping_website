using DotNetEnv;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using ProductService.API.Application.Services;
using ProductService.API.Application.Shared.Constant;
using ProductService.API.Infrastructure.Database;
using ProductService.API.Infrastructure.Repository.Product;

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

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IProductService, ProductServices>();

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

app.MapControllers();

app.Run();
