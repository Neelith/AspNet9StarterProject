using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YourProjectName.Application.Infrastructure.Caching;
using YourProjectName.Application.Infrastructure.Persistance;
using YourProjectName.Domain.WeatherForecasts;
using YourProjectName.Infrastructure.Caching;
using YourProjectName.Infrastructure.Persistence;
using YourProjectName.Infrastructure.Persistence.Repository;

namespace YourProjectName.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string? dbConnectionString, RedisSettings? redisSettings = default)
    {
        //Register infrastructure services here
        ArgumentNullException.ThrowIfNull(dbConnectionString, nameof(dbConnectionString));

        services.AddDbContext(dbConnectionString)
                .AddRepositories()
                .AddRedis(redisSettings);

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services) 
    {
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString));
        }

        services.AddDbContext<ApplicationDbContext>((options) => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, RedisSettings? redisSettings)
    {
        //Add redis only if we have a proper connection string configured
        //Otherwise, use the in-memory cache
        if (redisSettings is null || string.IsNullOrEmpty(redisSettings.ConnectionString))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = redisSettings.KeyPrefix;
            });
        }

        services.AddSingleton<IRedisCache, RedisCache>();

        return services;
    }
}
