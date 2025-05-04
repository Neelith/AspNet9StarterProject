using YourProjectName.Domain.Commons;

namespace YourProjectName.Domain.WeatherForecast;

public class WeatherForecastAggregate : Entity<int>, IAggregateRoot
{
    public DateOnly Date { get; private set; }
    public int TemperatureC { get; private set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; private set; }

    private WeatherForecastAggregate() { }

    public static WeatherForecastAggregate Create(DateOnly date, int temperatureC, string? summary)
    {
        return new WeatherForecastAggregate
        {
            Date = date,
            TemperatureC = temperatureC,
            Summary = summary
        };
    }
}
