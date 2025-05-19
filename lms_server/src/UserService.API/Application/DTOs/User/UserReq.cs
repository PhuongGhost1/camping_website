namespace UserService.API.Application.DTOs.User;

public record UpdateUserInfoReq
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}

public record UpdateUserPwdReq
{
    public required string NewPassword { get; init; }
    public required string ConfirmPassword { get; init; }
}