using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

namespace YourProjectName.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //Register application services here
        var assembly = typeof(DependencyInjection).Assembly;

        //Add FluentValidation
        services.AddValidatorsFromAssembly(assembly)
                .AddHandlers();

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IGetWeatherForecastHandler, GetWeatherForecastHandler>();

        return services;
    }
}
