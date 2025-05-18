using FluentValidation;
using Microsoft.Extensions.Logging;
using YourProjectName.Application.Commons.Handlers;
using YourProjectName.Application.Infrastructure.Caching;
using YourProjectName.Domain.WeatherForecasts;
using YourProjectName.Shared.Results;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public interface IGetWeatherForecastHandler : IHandler<GetWeatherForecastQuery, Result<GetWeatherForecastResponse>> { }

public sealed class GetWeatherForecastQueryHandler(
    ILogger<GetWeatherForecastQueryHandler> logger,
    IValidator<GetWeatherForecastQuery> validator,
    IWeatherForecastRepository weatherForecastRepository,
    IRedisCache redisCache)
    : IGetWeatherForecastHandler
{
    public async Task<Result<GetWeatherForecastResponse>> HandleAsync(GetWeatherForecastQuery request)
    {
        logger.LogInformation("Handling GetWeatherForecastQuery with request: {Request}", request);

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Result.Fail<GetWeatherForecastResponse>(validationResult.Errors);
        }

        const string cacheKey = "weatherforecasts";

        var cachedForecasts = await redisCache.GetAsync<List<Domain.WeatherForecasts.WeatherForecast>>(cacheKey);

        if (cachedForecasts is not null)
        {
            return GetWeatherForecastResponse.Create(cachedForecasts);
        }

        //string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        var forecasts = await weatherForecastRepository
            .GetWeatherForecasts(request?.TemperatureRangeMin, request?.TemperatureRangeMax);

        var response = GetWeatherForecastResponse.Create(forecasts);

        await redisCache.SetAsync(cacheKey, forecasts, TimeSpan.FromMinutes(2));

        return response;
    }
}
