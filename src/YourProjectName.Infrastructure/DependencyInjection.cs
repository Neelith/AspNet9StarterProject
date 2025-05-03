using Microsoft.Extensions.DependencyInjection;
using YourProjectName.Application.Infrastructure.Persistance;
using YourProjectName.Infrastructure.Persistance;

namespace YourProjectName.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string? dbConnectionString)
    {
        //Register infrastructure services here
        var assembly = typeof(DependencyInjection).Assembly;

        //services.AddDbContext(dbConnectionString);

        return services;
    }

    private static void AddDbContext(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            //Add your DB provider here
            //options.UseSqlServer(sqlServerConnectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}
