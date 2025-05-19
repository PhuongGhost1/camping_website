namespace UserService.API.Application.DTOs.User;

public record UpdateUserResp
{
    public int Status { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
}