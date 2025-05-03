using FluentResults;
using FluentValidation;
using YourProjectName.Application.Commons.Handlers;
using YourProjectName.Application.Infrastructure.Persistance;
using YourProjectName.Domain.Commons;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public interface IGetWeatherForecastHandler : IHandler<GetWeatherForecastQuery, Result<GetWeatherForecastResponse>> { }

public sealed class GetWeatherForecastQueryHandler(IValidator<GetWeatherForecastQuery> validator, IApplicationDbContext dbContext) : IGetWeatherForecastHandler
{
    public async Task<Result<GetWeatherForecastResponse>> HandleAsync(GetWeatherForecastQuery request)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(error => new ValidationError(error.ErrorMessage)).ToList();
            return Result.Fail(errors);
        }

        var db = dbContext;

        string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        var forecasts = Enumerable.Range(1, 5)
            .Select(index => 
            WeatherForecastAggregate.Create(
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)), 
                Random.Shared.Next(-20, 55), 
                summaries[Random.Shared.Next(summaries.Length)]))
            .ToArray();

        if (request is not null && request.TemperatureRangeMin.HasValue)
        {
            forecasts = forecasts.Where(x => x.TemperatureC >= request.TemperatureRangeMin.Value).ToArray();
        }

        if (request is not null && request.TemperatureRangeMax.HasValue)
        {
            forecasts = forecasts.Where(x => x.TemperatureC <= request.TemperatureRangeMax.Value).ToArray();
        }

        var response = new GetWeatherForecastResponse
        {
            Data = new GetWeatherForecastDataResponse(forecasts)
        };

        return Result.Ok(response);
    }
}
