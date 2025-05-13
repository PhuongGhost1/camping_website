using AuthenticationService.API.Application.Shared.Constant.Type;

namespace AuthenticationService.API.Domain;

public partial class Refreshtokens : BaseEntity
{
    public string? Token { get; set; }

    public DateTime ExpirationDate { get; set; }

    public Guid? UserId { get; set; }
}
