using Carter;
using Scalar.AspNetCore;
using YourProjectName.Application;
using YourProjectName.Infrastructure;

namespace YourProjectName.WebApi;

public static class DependencyInjection
{
    #region services configuration

    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration) 
    {
        //Get the database connection string
        string? dbConnectionString = configuration.GetConnectionString("TODO");

        //Register Web API services here
        services.AddApplicationServices()
                .AddInfrastructureServices(dbConnectionString)
                .AddCarter()
                .AddOpenApiServices();

        return services;
    }

    private static IServiceCollection AddOpenApiServices(this IServiceCollection services) 
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();

        return services;
    }

    #endregion
    #region services usage

    public static void UseAppServices(this WebApplication app) 
    {
        // Configure the HTTP request pipeline.

        //Discover and register all carter modules (endpoints)
        app.MapCarter();

        //Enable OpenApi documentation and UI
        app.UseOpenApi();

        app.UseHttpsRedirection();
    }

    private static void UseOpenApi(this WebApplication app) 
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            //Enables OpenApi UI using Scalar
            //Go to localhost:7232/scalar/v1 to access the UI
            app.MapScalarApiReference();
        }
    }

    #endregion
}
