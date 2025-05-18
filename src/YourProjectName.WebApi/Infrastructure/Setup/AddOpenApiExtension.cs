using Scalar.AspNetCore;

namespace YourProjectName.WebApi.Infrastructure.Setup;

internal static class AddOpenApiExtension
{
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }

    public static void UseOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            //Enables OpenApi UI using Scalar
            //Go to localhost:7232/scalar/v1 to access the UI
            app.MapScalarApiReference();
        }
    }
}
