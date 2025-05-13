using AuthenticationService.API.Application.Shared.Constant.Type;

namespace AuthenticationService.API.Domain;

public partial class Users : BaseEntity
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? AvatarUrl { get; set; }
}
