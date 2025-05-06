using FluentResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Application.Commons.Handlers;
using YourProjectName.Application.Infrastructure.Persistance;
using YourProjectName.Domain.Commons;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public interface IGetWeatherForecastHandler : IHandler<GetWeatherForecastQuery, Result<GetWeatherForecastResponse>> { }

public sealed class GetWeatherForecastQueryHandler(
    IValidator<GetWeatherForecastQuery> validator, 
    IWeatherForecastRepository weatherForecastRepository) 
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


        string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        var forecasts = await weatherForecastRepository
            .GetWeatherForecasts(request?.TemperatureRangeMin, request?.TemperatureRangeMax);

        var response = GetWeatherForecastResponse.Create(forecasts);

        return Result.Ok(response);
    }
}
