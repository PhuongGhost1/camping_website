using UserService.API.Application.Bindings;
using UserService.API.Application.DTOs.User;
using UserService.API.Application.Services;
using UserService.API.Application.Validators;

namespace UserService.API.Application.Endpoints;
public static class UserEndpoints
{   
    public static RouteGroupBuilder MapUserEndPoints(this RouteGroupBuilder group)
    {
        var userGroup = group.WithTags("User");

        group.MapGet("/user-info", async (IUserServices userServices, UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var userInfo = await userServices.GetUserInfo(userId);
            return Results.Ok(userInfo);
        })
        .WithName("GetUserInfo")
        .RequireAuthorization();

        group.MapPut("/update-info", async (IUserServices userServices, UpdateUserInfoReq req, UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var updateUserInfo = await userServices.UpdateUserInfo(userId.Value, req);
            return Results.Ok(updateUserInfo);
        })
        .WithName("UpdateUserInfo")
        .RequireAuthorization()
        .WithValidation<UpdateUserInfoReq>();

        group.MapPut("/update-pwd", async (IUserServices userServices, UpdateUserPwdReq req, UserId? userId) =>
        {
            if (userId is null) return Results.Unauthorized();
            var updateUserPwd = await userServices.UpdateUserPwd(userId.Value, req);
            return Results.Ok(updateUserPwd);
        })
        .WithName("UpdateUserPwd")
        .RequireAuthorization()
        .WithValidation<UpdateUserPwdReq>();

        return group;
    }
}