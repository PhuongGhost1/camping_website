using UserService.API.Application.Shared.Type;

namespace UserService.API.Domain;

public partial class Users : BaseEntity
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();
}
