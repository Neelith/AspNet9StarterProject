using FluentResults;
using FluentValidation;
using YourProjectName.Application.Commons;
using YourProjectName.Domain.Commons;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed record GetWeatherForecastQuery(
    int? TemperatureRangeMin, 
    int? TemperatureRangeMax) 
    : IQuery {}

public sealed class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
{
    public GetWeatherForecastQueryValidator()
    {
        RuleFor(x => x.TemperatureRangeMin)
            .GreaterThanOrEqualTo(-20)
            .LessThanOrEqualTo(55)
            .When(x => x.TemperatureRangeMin.HasValue);

        RuleFor(x => x.TemperatureRangeMax)
            .GreaterThanOrEqualTo(-20)
            .LessThanOrEqualTo(55)
            .When(x => x.TemperatureRangeMax.HasValue);
    }
}

public sealed record GetWeatherForecastResponse() : DataResponse<GetWeatherForecastDataResponse>;

public sealed record GetWeatherForecastDataResponse(IEnumerable<Domain.WeatherForecast.WeatherForecast> Forecasts);

public sealed class GetWeatherForecastHandler(IValidator<GetWeatherForecastQuery> validator) : IGetWeatherForecastHandler
{
    //validate the query using FluentValidation and manage automatic registration for handlers in the DI container
    public async Task<Result<GetWeatherForecastResponse>> GetWeatherForecast(GetWeatherForecastQuery query)
    {
        var validationResult = await validator.ValidateAsync(query);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(error => new ValidationError(error.ErrorMessage)).ToList();
            return Result.Fail(errors);
        }

        string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        var forecasts = Enumerable.Range(1, 5).Select(index =>
            new Domain.WeatherForecast.WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();

        if (query is not null && query.TemperatureRangeMin.HasValue)
        {
            forecasts = forecasts.Where(x => x.TemperatureC >= query.TemperatureRangeMin.Value).ToArray();
        }

        if (query is not null && query.TemperatureRangeMax.HasValue)
        {
            forecasts = forecasts.Where(x => x.TemperatureC <= query.TemperatureRangeMax.Value).ToArray();
        }

        var response = new GetWeatherForecastResponse
        {
            Data = new GetWeatherForecastDataResponse(forecasts)
        };

        return Result.Ok(response);
    }
}
