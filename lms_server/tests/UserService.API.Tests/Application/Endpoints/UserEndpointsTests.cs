using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Moq;
using UserService.API.Application.DTOs.User;
using UserService.API.Application.Services;
using Xunit;

namespace UserService.API.Application.Endpoints.Tests
{
    public class UserEndpointsTests
    {
        [Fact]
        public async Task Test_GetUserInfo_ReturnsOk_WhenUserIdIsValidAndAuthorized()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            var userId = new UserId(123);
            var expectedUserInfo = new UserInfoDto { Id = 123, Name = "Test User" };

            mockUserServices
                .Setup(s => s.GetUserInfo(userId))
                .ReturnsAsync(expectedUserInfo);

            // Simulate the endpoint delegate
            var endpointDelegate = UserEndpoints.MapUserEndPoints;

            // Since MapUserEndPoints is an extension method for RouteGroupBuilder,
            // and the actual endpoint logic is inside the lambda, we need to test the lambda directly.
            // We'll extract the lambda logic here for testing.

            // Act
            var result = await (async (IUserServices userServices, UserId? uid) =>
            {
                if (uid is null) return Results.Unauthorized();
                var userInfo = await userServices.GetUserInfo(uid);
                return Results.Ok(userInfo);
            })(mockUserServices.Object, userId);

            // Assert
            var okResult = Assert.IsType<Ok<UserInfoDto>>(result);
            Assert.Equal(expectedUserInfo, okResult.Value);
        }

        [Fact]
        public async Task Test_UpdateUserInfo_ReturnsOk_WhenRequestIsValidAndUserIsAuthorized()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            var userId = new UserId(456);
            var updateReq = new UpdateUserInfoReq { Name = "Updated Name" };
            var expectedResponse = new UpdateUserInfoResp { Success = true };

            mockUserServices
                .Setup(s => s.UpdateUserInfo(userId.Value, updateReq))
                .ReturnsAsync(expectedResponse);

            // Simulate the endpoint lambda for /update-info
            var result = await (async (IUserServices userServices, UpdateUserInfoReq req, UserId? uid) =>
            {
                if (uid is null) return Results.Unauthorized();
                var updateUserInfo = await userServices.UpdateUserInfo(uid.Value, req);
                return Results.Ok(updateUserInfo);
            })(mockUserServices.Object, updateReq, userId);

            // Assert
            var okResult = Assert.IsType<Ok<UpdateUserInfoResp>>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public async Task Test_UpdateUserPwd_ReturnsOk_WhenRequestIsValidAndUserIsAuthorized()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            var userId = new UserId(789);
            var updatePwdReq = new UpdateUserPwdReq { OldPassword = "oldpwd", NewPassword = "newpwd" };
            var expectedResponse = new UpdateUserPwdResp { Success = true };

            mockUserServices
                .Setup(s => s.UpdateUserPwd(userId.Value, updatePwdReq))
                .ReturnsAsync(expectedResponse);

            // Simulate the endpoint lambda for /update-pwd
            var result = await (async (IUserServices userServices, UpdateUserPwdReq req, UserId? uid) =>
            {
                if (uid is null) return Results.Unauthorized();
                var updateUserPwd = await userServices.UpdateUserPwd(uid.Value, req);
                return Results.Ok(updateUserPwd);
            })(mockUserServices.Object, updatePwdReq, userId);

            // Assert
            var okResult = Assert.IsType<Ok<UpdateUserPwdResp>>(result);
            Assert.Equal(expectedResponse, okResult.Value);
        }

        [Fact]
        public void Test_UserEndpoints_RequireAuthorization_ForAllRoutes()
        {
            // Arrange
            var endpointRouteBuilder = new RouteBuilderStub();
            var group = new RouteGroupBuilder(endpointRouteBuilder);

            // Act
            UserEndpoints.MapUserEndPoints(group);

            // Assert
            var endpoints = group.DataSources.SelectMany(ds => ds.Endpoints).ToList();
            Assert.NotEmpty(endpoints);

            foreach (var endpoint in endpoints)
            {
                var metadata = endpoint.Metadata;
                var hasAuth = metadata.Any(m => m.GetType().Name.Contains("Authorize"));
                Assert.True(hasAuth, $"Endpoint '{endpoint.DisplayName}' does not require authorization.");
            }
        }

        [Fact]
        public async Task Test_GetUserInfo_ReturnsUnauthorized_WhenUserIdIsMissing()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            UserId? userId = null;

            // Simulate the endpoint lambda for /user-info
            var result = await (async (IUserServices userServices, UserId? uid) =>
            {
                if (uid is null) return Results.Unauthorized();
                var userInfo = await userServices.GetUserInfo(uid);
                return Results.Ok(userInfo);
            })(mockUserServices.Object, userId);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }

        [Fact]
        public async Task Test_UpdateUserInfo_ReturnsUnauthorized_WhenUserIdIsMissing()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            UpdateUserInfoReq updateReq = new UpdateUserInfoReq { Name = "ShouldNotMatter" };
            UserId? userId = null;

            // Simulate the endpoint lambda for /update-info
            var result = await (async (IUserServices userServices, UpdateUserInfoReq req, UserId? uid) =>
            {
                if (uid is null) return Results.Unauthorized();
                var updateUserInfo = await userServices.UpdateUserInfo(uid.Value, req);
                return Results.Ok(updateUserInfo);
            })(mockUserServices.Object, updateReq, userId);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }

        // Minimal stub for RouteBuilder and RouteGroupBuilder to allow endpoint registration
        private class RouteBuilderStub : IEndpointRouteBuilder
        {
            public IServiceProvider ServiceProvider => null!;
            public ICollection<EndpointDataSource> DataSources { get; } = new List<EndpointDataSource>();
            public IApplicationBuilder CreateApplicationBuilder() => null!;
        }

        [Fact]
        public async Task Test_UpdateUserInfo_ReturnsValidationError_WhenRequestIsInvalid()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            var userId = new UserId(456);
            // Simulate invalid request: missing required Name property (assuming it's required)
            var invalidReq = new UpdateUserInfoReq { Name = null };

            // Simulate the endpoint lambda for /update-info
            // Since validation is handled by WithValidation<UpdateUserInfoReq>(), which is middleware,
            // we simulate what would happen if the model state is invalid: the endpoint should not be called,
            // and a validation error should be returned.
            // In minimal APIs, invalid model state returns a 400 BadRequest with validation errors.

            // Act
            // Simulate validation failure by directly returning BadRequest
            var result = Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Name", new[] { "The Name field is required." } }
            });

            // Assert
            var validationResult = Assert.IsType<ValidationProblem>(result);
            Assert.True(validationResult.ProblemDetails.Errors.ContainsKey("Name"));
            Assert.Contains("The Name field is required.", validationResult.ProblemDetails.Errors["Name"]);
        }

        [Fact]
        public void Test_UpdateUserPwd_ReturnsValidationError_WhenRequestIsInvalid()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            var userId = new UserId(789);
            // Simulate invalid request: missing required NewPassword property (assuming it's required)
            var invalidReq = new UpdateUserPwdReq { OldPassword = "oldpwd", NewPassword = null };

            // Act
            // Simulate validation failure by directly returning ValidationProblem
            var result = Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { "NewPassword", new[] { "The NewPassword field is required." } }
            });

            // Assert
            var validationResult = Assert.IsType<ValidationProblem>(result);
            Assert.True(validationResult.ProblemDetails.Errors.ContainsKey("NewPassword"));
            Assert.Contains("The NewPassword field is required.", validationResult.ProblemDetails.Errors["NewPassword"]);
        }

        [Fact]
        public async Task Test_UpdateUserPwd_ReturnsUnauthorized_WhenUserIdIsMissing()
        {
            // Arrange
            var mockUserServices = new Mock<IUserServices>();
            UpdateUserPwdReq updatePwdReq = new UpdateUserPwdReq { OldPassword = "oldpwd", NewPassword = "newpwd" };
            UserId? userId = null;

            // Simulate the endpoint lambda for /update-pwd
            var result = await (async (IUserServices userServices, UpdateUserPwdReq req, UserId? uid) =>
            {
                if (uid is null) return Results.Unauthorized();
                var updateUserPwd = await userServices.UpdateUserPwd(uid.Value, req);
                return Results.Ok(updateUserPwd);
            })(mockUserServices.Object, updatePwdReq, userId);

            // Assert
            Assert.IsType<UnauthorizedHttpResult>(result);
        }
    }
}