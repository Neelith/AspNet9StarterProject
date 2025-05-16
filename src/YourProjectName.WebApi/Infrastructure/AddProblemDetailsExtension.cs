namespace YourProjectName.WebApi.Infrastructure;

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

                        var method = httpContext.Request.Method;
                        context.ProblemDetails.Extensions.TryAdd("method", method);

                        var requestHeaders = httpContext.Request.Headers;
                        context.ProblemDetails.Extensions.TryAdd("requestHeaders", requestHeaders);
                    });
    }
}
