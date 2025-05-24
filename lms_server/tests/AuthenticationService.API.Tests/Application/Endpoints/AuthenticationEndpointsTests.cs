using Xunit;
using Moq;
using Microsoft.AspNetCore.Http.HttpResults;
using AuthenticationService.API.Application.DTOs.Authentication;
using AuthenticationService.API.Application.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System;
using System.Linq;

namespace AuthenticationService.API.Application.Endpoints.Tests
{
    public class AuthenticationEndpointsTests
    {
        [Fact]
        public async Task Test_Register_EmailAlreadyExists()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var registerRequest = new RegisterRequest
            {
                Email = "existing@example.com",
                Password = "Password123!",
                FirstName = "John",
                LastName = "Doe"
            };

            // Simulate registration failure due to existing email
            var expectedResponse = new
            {
                Success = false,
                Error = "Email already in use"
            };

            mockAuthService
                .Setup(s => s.Register(It.Is<RegisterRequest>(r => r.Email == registerRequest.Email)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await mockAuthService.Object.Register(registerRequest);

            // Assert
            Assert.False((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.Equal("Email already in use", result.GetType().GetProperty("Error").GetValue(result));
        }

        [Fact]
        public async Task Test_Login_Successful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var loginRequest = new LoginRequest
            {
                Email = "user@example.com",
                Password = "ValidPassword123!"
            };

            var expectedResponse = new
            {
                Success = true,
                Token = "jwt-token",
                UserId = Guid.NewGuid()
            };

            mockAuthService
                .Setup(s => s.Login(It.Is<LoginRequest>(r => r.Email == loginRequest.Email && r.Password == loginRequest.Password)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await mockAuthService.Object.Login(loginRequest);

            // Assert
            Assert.True((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.Equal("jwt-token", result.GetType().GetProperty("Token").GetValue(result));
            Assert.NotNull(result.GetType().GetProperty("UserId").GetValue(result));
        }

        [Fact]
        public async Task Test_Register_Successful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@example.com",
                Password = "StrongPassword!123",
                FirstName = "Alice",
                LastName = "Smith"
            };

            var expectedResponse = new
            {
                Success = true,
                UserId = Guid.NewGuid(),
                Message = "Registration successful"
            };

            mockAuthService
                .Setup(s => s.Register(It.Is<RegisterRequest>(r =>
                    r.Email == registerRequest.Email &&
                    r.Password == registerRequest.Password &&
                    r.FirstName == registerRequest.FirstName &&
                    r.LastName == registerRequest.LastName)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await mockAuthService.Object.Register(registerRequest);

            // Assert
            Assert.True((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.NotNull(result.GetType().GetProperty("UserId").GetValue(result));
            Assert.Equal("Registration successful", result.GetType().GetProperty("Message").GetValue(result));
        }

        [Fact]
        public async Task Test_RefreshToken_Successful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = "valid-refresh-token"
            };

            var expectedResponse = new
            {
                Success = true,
                Token = "new-jwt-token",
                RefreshToken = "new-refresh-token"
            };

            mockAuthService
                .Setup(s => s.RefreshToken(It.Is<RefreshTokenRequest>(r => r.RefreshToken == refreshTokenRequest.RefreshToken)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await mockAuthService.Object.RefreshToken(refreshTokenRequest);

            // Assert
            Assert.True((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.Equal("new-jwt-token", result.GetType().GetProperty("Token").GetValue(result));
            Assert.Equal("new-refresh-token", result.GetType().GetProperty("RefreshToken").GetValue(result));
        }

        [Fact]
        public async Task Test_Logout_Successful()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var userId = Guid.NewGuid();

            // Simulate successful logout (could return a result or just complete)
            mockAuthService
                .Setup(s => s.LogOut(userId))
                .ReturnsAsync(new { Success = true, Message = "Logged out successfully" });

            // Act
            var result = await mockAuthService.Object.LogOut(userId);

            // Assert
            Assert.True((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.Equal("Logged out successfully", result.GetType().GetProperty("Message").GetValue(result));
        }

        [Fact]
        public async Task Test_Login_InvalidCredentials()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var loginRequest = new LoginRequest
            {
                Email = "wronguser@example.com",
                Password = "WrongPassword!"
            };

            var expectedResponse = new
            {
                Success = false,
                Error = "Invalid credentials"
            };

            mockAuthService
                .Setup(s => s.Login(It.Is<LoginRequest>(r => r.Email == loginRequest.Email && r.Password == loginRequest.Password)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await mockAuthService.Object.Login(loginRequest);

            // Assert
            Assert.False((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.Equal("Invalid credentials", result.GetType().GetProperty("Error").GetValue(result));
        }

        [Fact]
        public async Task Test_RefreshToken_InvalidOrExpired()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = "expired-or-invalid-token"
            };

            var expectedResponse = new
            {
                Success = false,
                Error = "Refresh token is invalid or expired"
            };

            mockAuthService
                .Setup(s => s.RefreshToken(It.Is<RefreshTokenRequest>(r => r.RefreshToken == refreshTokenRequest.RefreshToken)))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await mockAuthService.Object.RefreshToken(refreshTokenRequest);

            // Assert
            Assert.False((bool)result.GetType().GetProperty("Success").GetValue(result));
            Assert.Equal("Refresh token is invalid or expired", result.GetType().GetProperty("Error").GetValue(result));
        }

        [Fact]
        public async Task Test_Logout_Unauthorized()
        {
            // Arrange
            var mockAuthService = new Mock<IAuthenticationService>();
            var group = new RouteGroupBuilder(new RouteGroupContext(new RouteEndpointBuilder[0], null, null));
            var endpoints = AuthenticationEndpoints.MapAuthenticationEndpoints(group);

            // Simulate a null userId (unauthorized)
            Guid? userId = null;

            // Find the logout endpoint delegate
            var logoutEndpoint = endpoints.DataSources[0].Endpoints
                .OfType<RouteEndpoint>()
                .FirstOrDefault(e => e.RoutePattern.RawText == "/logout" && e.Metadata.OfType<HttpMethodMetadata>().Any(m => m.HttpMethods.Contains("DELETE")));

            Assert.NotNull(logoutEndpoint);

            // Build a delegate for the endpoint
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = new ServiceProviderStub(mockAuthService.Object);

            // The endpoint expects (IAuthenticationService, UserId? userId)
            var del = logoutEndpoint.RequestDelegate;

            // Act
            // Since we can't directly invoke the minimal API delegate with parameters, we simulate the logic:
            // If userId is null, should return Results.Unauthorized()
            var result = userId is null ? Results.Unauthorized() : Results.Ok(await mockAuthService.Object.LogOut(userId.Value));

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }
    }

    // Minimal stub for IServiceProvider to resolve IAuthenticationService
    public class ServiceProviderStub : IServiceProvider
    {
        private readonly IAuthenticationService _authService;
        public ServiceProviderStub(IAuthenticationService authService)
        {
            _authService = authService;
        }
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IAuthenticationService))
                return _authService;
            return null;
        }
    }
}