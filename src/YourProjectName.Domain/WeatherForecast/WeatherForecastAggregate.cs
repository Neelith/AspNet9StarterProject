using FluentResults;
using YourProjectName.Domain.Commons;

namespace YourProjectName.Domain.WeatherForecast;

public class WeatherForecastAggregate : AggregateRoot<int>
{
    public DateOnly Date { get; private set; }
    public int TemperatureC { get; private set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public Summary? Summary { get; private set; }

    private WeatherForecastAggregate() { }

    public static Result<WeatherForecastAggregate> Create(DateOnly date, int temperatureC, string? summaryValue)
    {
        var summaryCreationResult = CreateSummary(summaryValue);

        if (summaryCreationResult is { IsFailed: true })
        {
            return Result.Fail(summaryCreationResult.Errors);
        }

        return new WeatherForecastAggregate
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summaryCreationResult.Value
        };
    }

    private static Result<Summary?> CreateSummary(string? summaryValue)
    {
        if (string.IsNullOrWhiteSpace(summaryValue))
        {
            return Result.Ok();
        }

        var summaryCreationResult = Summary.Create(summaryValue);

        if (summaryCreationResult.IsFailed)
        {
            return Result.Fail(summaryCreationResult.Errors);
        }

        return summaryCreationResult!;
    }
}
