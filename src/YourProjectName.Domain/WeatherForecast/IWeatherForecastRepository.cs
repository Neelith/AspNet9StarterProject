namespace YourProjectName.Domain.WeatherForecast;
public interface IWeatherForecastRepository
{
    Task<List<WeatherForecastAggregate>> GetWeatherForecasts(int? temperatureRangeMin, int? temperatureRangeMax);
}
