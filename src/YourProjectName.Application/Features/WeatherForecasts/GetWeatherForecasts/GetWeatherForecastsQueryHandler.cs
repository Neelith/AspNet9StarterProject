using FluentValidation;
using Microsoft.Extensions.Logging;
using YourProjectName.Application.Commons.Handlers;
using YourProjectName.Application.Infrastructure.Caching;
using YourProjectName.Domain.WeatherForecasts;
using YourProjectName.Shared.Results;

namespace YourProjectName.Application.Features.WeatherForecasts.GetWeatherForecasts;

public interface IGetWeatherForecastHandler : IHandler<GetWeatherForecastsQuery, Result<GetWeatherForecastsResponse>> { }

public sealed class GetWeatherForecastsQueryHandler(
    ILogger<GetWeatherForecastsQueryHandler> logger,
    IValidator<GetWeatherForecastsQuery> validator,
    IWeatherForecastRepository weatherForecastRepository,
    IRedisCache redisCache)
    : IGetWeatherForecastHandler
{
    public async Task<Result<GetWeatherForecastsResponse>> HandleAsync(GetWeatherForecastsQuery request)
    {
        logger.LogInformation("Handling GetWeatherForecastQuery with request: {Request}", request);

        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return Result.Fail<GetWeatherForecastsResponse>(validationResult.Errors);
        }

        const string cacheKey = "weatherforecasts";

        var cachedForecasts = await redisCache.GetAsync<List<WeatherForecast>>(cacheKey);

        if (cachedForecasts is not null)
        {
            return GetWeatherForecastsResponse.Create(cachedForecasts);
        }

        //string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        var forecasts = await weatherForecastRepository
            .GetWeatherForecasts(request?.TemperatureRangeMin, request?.TemperatureRangeMax);

        var response = GetWeatherForecastsResponse.Create(forecasts);

        await redisCache.SetAsync(cacheKey, forecasts, TimeSpan.FromMinutes(2));

        return response;
    }
}
