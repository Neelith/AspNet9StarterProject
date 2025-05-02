using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scalar.AspNetCore;
using YourProjectName.Application;
using YourProjectName.Infrastructure;
using YourProjectName.WebApi.Commons;
using YourProjectName.WebApi.Middlewares;

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
                .AddEndpoints(Assembly.GetExecutingAssembly())
                .AddOpenApiServices()
                .AddMiddlewares();

        return services;
    }

    // Add the custom middlewares to the pipeline
    private static IServiceCollection AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ValidationMiddleware>();

        return services;
    }

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    private static IServiceCollection AddOpenApiServices(this IServiceCollection services) 
    {
        services.AddOpenApi();

        return services;
    }

    //Discover all endpoints that implement the IEndpoints interface
    private static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpointServiceDescriptors = [.. assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoints)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoints), type))];

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    #endregion
    #region services usage

    // Configure the HTTP request pipeline.
    public static void UseAppServices(this WebApplication app) 
    {
        //Register all the endpoints that implement the IEndpoints interface
        app.MapEndpoints();

        //Enable OpenApi documentation and UI
        app.UseOpenApi();

        app.UseHttpsRedirection();

        //Enable validation middleware
        app.UseMiddleware<ValidationMiddleware>();
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

    private static void MapEndpoints(this WebApplication app) 
    {
        var endpointGroups = app.Services.GetRequiredService<IEnumerable<IEndpoints>>();

        foreach (var endpointGroup in endpointGroups)
        {
            endpointGroup.AddRoutes(app);
        }
    }

    #endregion
}
