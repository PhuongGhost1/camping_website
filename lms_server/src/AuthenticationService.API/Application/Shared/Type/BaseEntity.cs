namespace AuthenticationService.API.Application.Shared.Constant.Type;
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
