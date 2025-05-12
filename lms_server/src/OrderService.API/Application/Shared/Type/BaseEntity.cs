namespace OrderService.API.Application.Shared.Type;
public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
