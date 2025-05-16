using AuthenticationService.API.Infrastructure.Database;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using AuthenticationService.API.Application.Shared.Constant;
using AuthenticationService.API.Infrastructure.Repository.Authentication;
using AuthenticationService.API.Application.Services;
using AuthenticationService.API.Core.Jwt;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using AuthenticationService.API.Infrastructure.Messaging.Publisher;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

string envPath = Path.GetFullPath(Path.Combine
            (AppDomain.CurrentDomain.BaseDirectory, "../../../../../../lms_server/.env"));
Env.Load(envPath);

string rabbitHost = RabbitMQConst.RABBITMQ_HOST!;
string rabbitPort = RabbitMQConst.RABBITMQ_PORT!;
string rabbitUser = RabbitMQConst.RABBITMQ_USERNAME!;
string rabbitPass = RabbitMQConst.RABBITMQ_PASSWORD!;

// Tạo RabbitMQ URI
string rabbitUri = $"amqp://{rabbitUser}:{rabbitPass}@{rabbitHost}:{rabbitPort}/";

// Add Health check
builder.Services.AddHealthChecks()
    .AddMySql(
        connectionString: MySQLDatabase.DB_CONNECTION_STRING!,
        name: "mysql",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "sql" })
    // .AddRedis(
    //     redisConn,
    //     name: "redis",
    //     failureStatus: HealthStatus.Unhealthy,
    //     tags: new[] { "cache", "redis" })
    .AddRabbitMQ(
        rabbitConnectionString: rabbitUri,
        name: "rabbitmq",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "queue", "rabbitmq" });

builder.Services.AddHealthChecksUI(options =>
{
    options.SetHeaderText("Microservice Health Check Dashboard");
}).AddInMemoryStorage();

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

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConst.JWT_SECRET_KEY))
        };
        options.Events = new JwtBearerEvents {
            OnAuthenticationFailed = context => {
                Console.WriteLine("JWT auth failed: " + context.Exception.Message);
                Console.WriteLine("Authorization header: " + context.Request.Headers["Authorization"]);
                return Task.CompletedTask;
            }
        };
        options.UseSecurityTokenValidators = true;
    }
);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticationService.API", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    }
);

builder.Services.AddDbContext<CampingDbContext>(options =>
{
    options.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));

});

builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationServices>();

builder.Services.AddSingleton<IEventPublisher, EventPublisher>();

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

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
