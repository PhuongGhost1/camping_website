namespace UserService.API.Application.Shared.Type;
public class BaseResp
{
    public int Code { get; set; }
    public string Message { get; set; } = null!;
}