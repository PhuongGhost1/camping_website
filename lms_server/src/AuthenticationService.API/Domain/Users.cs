using AuthenticationService.API.Application.Shared.Constant.Type;
using System;
using System.Collections.Generic;

namespace AuthenticationService.API.Domain;

public partial class Users : BaseEntity
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? AvatarUrl { get; set; }
}
