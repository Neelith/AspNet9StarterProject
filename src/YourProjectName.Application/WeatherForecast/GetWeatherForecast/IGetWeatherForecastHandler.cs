namespace YourProjectName.Application.WeatherForecast.GetWeatherForecast;

public interface IGetWeatherForecastHandler
{
    Task<GetWeatherForecastResponse> GetWeatherForecast(GetWeatherForecastQuery? query);
}
