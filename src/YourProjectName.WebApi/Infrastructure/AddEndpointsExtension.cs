using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using YourProjectName.WebApi.Commons;

namespace YourProjectName.WebApi.Infrastructure;

internal static class AddEndpointsExtension
{
    //Discover all endpoints that implement the IEndpoints interface
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] endpointServiceDescriptors = [.. assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoints)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoints), type))];

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    public static void MapEndpoints(this WebApplication app)
    {
        var endpointGroups = app.Services.GetRequiredService<IEnumerable<IEndpoints>>();

        foreach (var endpointGroup in endpointGroups)
        {
            endpointGroup.AddRoutes(app);
        }
    }
}
