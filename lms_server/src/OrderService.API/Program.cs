using System.Text;
using System.Text.Json.Serialization;
using DotNetEnv;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.API.Application.Services;
using OrderService.API.Application.Shared.Constant;
using OrderService.API.Infrastructure.Database;
using OrderService.API.Infrastructure.Messaging.Consumer;
using OrderService.API.Infrastructure.Repository;
using FluentValidation;
using OrderService.API.Application.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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

// builder.Services.AddControllers().AddJsonOptions(options =>
// {
//     options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
//     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
// });

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrderService.API", Version = "v1" });
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

// builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
// {
//     var configuration = ConfigurationOptions.Parse(RedisConstant.REDIS_CONNECTION_STRING);
//     configuration.AbortOnConnectFail = false;
//     return ConnectionMultiplexer.Connect(configuration);
// });

builder.Services.AddDbContext<CampingDbContext>(options =>
{
    options.UseMySql(MySQLDatabase.DB_CONNECTION_STRING, ServerVersion.Parse("8.0.34-mysql"));
});

builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IOrderItemServices, OrderItemServices>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

builder.Services.AddHostedService<RegisterEventConsumer>();
builder.Services.AddHostedService<PaymentCompletedEvent>();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7212, o =>
    {
        o.UseHttps();
        o.Protocols = HttpProtocols.Http1AndHttp2;
    });
});

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

// app.MapControllers();
app.MapGroup("api/orders")
    .RequireAuthorization()
    .MapOrderEndpoints();

app.Run();
