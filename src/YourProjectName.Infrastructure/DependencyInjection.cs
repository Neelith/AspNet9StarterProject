using Microsoft.Extensions.DependencyInjection;
using YourProjectName.Application.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Infrastructure.Persistence;

namespace YourProjectName.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string? dbConnectionString)
    {
        //Register infrastructure services here

        if (!string.IsNullOrEmpty(dbConnectionString))
        {
            //log warning if the connection string is empty
            services.AddDbContext(dbConnectionString);
        }


        return services;
    }

    private static void AddDbContext(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        services.AddDbContext<ApplicationDbContext>((options) => options.UseNpgsql(connectionString));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}
