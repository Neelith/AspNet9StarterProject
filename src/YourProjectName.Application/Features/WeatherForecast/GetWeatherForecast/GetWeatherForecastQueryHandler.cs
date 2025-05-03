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
    IApplicationDbContext dbContext) 
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

        var query = dbContext.Forecasts.AsNoTracking();

        if (request is not null && request.TemperatureRangeMin.HasValue)
        {
            query = query.Where(x => x.TemperatureC >= request.TemperatureRangeMin.Value);
        }

        if (request is not null && request.TemperatureRangeMax.HasValue)
        {
            query = query.Where(x => x.TemperatureC <= request.TemperatureRangeMax.Value);
        }

        var forecasts = await query
            .Select(x => WeatherForecastAggregate.Create(x.Date, x.TemperatureC, x.Summary))
            .ToListAsync();

        var response = new GetWeatherForecastResponse
        {
            Data = new GetWeatherForecastDataResponse(forecasts)
        };

        return Result.Ok(response);
    }
}
