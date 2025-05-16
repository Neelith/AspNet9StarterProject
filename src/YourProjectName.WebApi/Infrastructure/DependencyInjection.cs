using System.Reflection;
using YourProjectName.Application;
using YourProjectName.Infrastructure;
using YourProjectName.Infrastructure.Caching;

namespace YourProjectName.WebApi.Infrastructure;

internal static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Add problem details
        services.ConfigureProblemDetails();

        //Get the database connection string
        string? dbConnectionString = configuration.GetConnectionString("YourProjectNameDb");

        //Get the redis settings
        RedisSettings? redisSettings = services.AddRedisSettings(configuration);

        //Register Web API services here
        services.AddApplicationServices()
                .AddInfrastructureServices(dbConnectionString, redisSettings)
                .AddEndpoints(Assembly.GetExecutingAssembly())
                .AddOpenApiServices();

        return services;
    }

    // Configure the HTTP request pipeline.
    public static void UseAppServices(this WebApplication app)
    {
        //Register all the endpoints that implement the IEndpoints interface
        app.MapEndpoints();

        //Enable OpenApi documentation and UI
        app.UseOpenApi();

        app.UseHttpsRedirection();
    }
}
