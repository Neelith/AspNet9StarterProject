using FluentResults;
using FluentValidation;
using YourProjectName.Application.Commons.Handlers;
using YourProjectName.Application.Infrastructure.Caching;
using YourProjectName.Domain.Commons;
using YourProjectName.Domain.WeatherForecasts;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public interface IGetWeatherForecastHandler : IHandler<GetWeatherForecastQuery, Result<GetWeatherForecastResponse>> { }

public sealed class GetWeatherForecastQueryHandler(
    IValidator<GetWeatherForecastQuery> validator, 
    IWeatherForecastRepository weatherForecastRepository,
    IRedisCache redisCache) 
    : IGetWeatherForecastHandler
{
    public async Task<Result<GetWeatherForecastResponse>> HandleAsync(GetWeatherForecastQuery request)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(error => new ValidationError(error.ErrorMessage)).ToList();
            return Result.Fail(errors);
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

        //not awaited for performance reasons
        redisCache.SetAsync(cacheKey, forecasts, TimeSpan.FromMinutes(2));

        return response;
    }
}
