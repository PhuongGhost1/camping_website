using System.Security.Claims;

namespace PaymentService.API.Application.Bindings;

public readonly record struct UserId(Guid Value)
{
    public static ValueTask<UserId?> BindAsync(HttpContext httpContext)
    {
        var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is null || !Guid.TryParse(claim.Value, out var guid))
            return ValueTask.FromResult<UserId?>(null);

        return ValueTask.FromResult<UserId?>(new UserId(guid));
    }
    
    public static implicit operator Guid(UserId userId) => userId.Value;
}