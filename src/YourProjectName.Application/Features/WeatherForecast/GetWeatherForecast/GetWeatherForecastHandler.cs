using FluentResults;
using YourProjectName.Application.Commons;
using YourProjectName.Domain.Commons;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed class GetWeatherForecastQuery() : IQuery 
{
    public int? TemperatureRangeMin { get; init; } 
    public int? TemperatureRangeMax { get; init; }
}

public sealed class GetWeatherForecastResponse() : DataResponse<IEnumerable<WeatherForecastModel>>;

public sealed class GetWeatherForecastHandler : IGetWeatherForecastHandler
{
    //validate the query using FluentValidation and manage automatic registration for handlers in the DI container
    public Task<Result<GetWeatherForecastResponse>> GetWeatherForecast(GetWeatherForecastQuery? query)
    {
        if (query is null)
        {
            var error = Errors.ValidationError("Query cannot be null");
            return Task.FromResult(Result.Fail<GetWeatherForecastResponse>(error));
        }

        string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecastModel
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();

        if (query is not null && query.TemperatureRangeMin.HasValue)
        {
            forecast = forecast.Where(x => x.TemperatureC >= query.TemperatureRangeMin.Value).ToArray();
        }

        if (query is not null && query.TemperatureRangeMax.HasValue)
        {
            forecast = forecast.Where(x => x.TemperatureC <= query.TemperatureRangeMax.Value).ToArray();
        }

        var result = Result.Ok(new GetWeatherForecastResponse { Data = forecast });

        return Task.FromResult(result);
    }
}
