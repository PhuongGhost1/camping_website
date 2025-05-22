using FluentValidation;

namespace OrderService.API.Application.Validators;
public static class ValidationExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder)
        where T : class
    {
        return builder.AddEndpointFilter(async (context, next) =>
        {
            var arg = context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T)) as T;
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator != null && arg != null)
            {
                var result = await validator.ValidateAsync(arg);
                if (!result.IsValid)
                {
                    var errors = result.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Error = e.ErrorMessage
                    });
                    return Results.BadRequest(errors);
                }
            }

            return await next(context);
        });
    }
}
    