using System.Reflection;
using YourProjectName.Application;
using YourProjectName.Infrastructure;
using YourProjectName.Infrastructure.Caching;
using YourProjectName.WebApi.Infrastructure.Middlewares;

namespace YourProjectName.WebApi.Infrastructure.Setup;

internal static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this WebApplicationBuilder webApplicationBuilder)
    {
        //Add logging
        webApplicationBuilder.AddLogging();

        //Get the services collection
        IServiceCollection services = webApplicationBuilder.Services;

        //Add global exception handling
        services.AddExceptionHandler<GlobalExceptionHandler>();

        //Add problem details
        services.ConfigureProblemDetails();

        //Get configuration
        IConfiguration configuration = webApplicationBuilder.Configuration;

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
        //Enable logging
        app.UseLogging();

        //Enable global exception handling
        app.UseExceptionHandler();

        //Register all the endpoints that implement the IEndpoints interface
        app.MapEndpoints();

        //Enable OpenApi documentation and UI
        app.UseOpenApi();

        app.UseHttpsRedirection();
    }
}
