namespace YourProjectName.WebApi.Infrastructure.Setup;

internal static class AddProblemDetailsExtension
{
    public static void ConfigureProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
                    options.CustomizeProblemDetails = context =>
                    {
                        var httpContext = context.HttpContext;

                        var instance = httpContext.Request.Path;
                        context.ProblemDetails.Instance = instance;

                        if (context.Exception is null)
                        {
                            
                            var method = httpContext.Request.Method;
                            context.ProblemDetails.Extensions.TryAdd("method", method);

                            context.ProblemDetails.Extensions.TryAdd("endpoint", $"{method} {instance}");

                        }

                    });
    }
}
