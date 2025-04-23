using FluentResults;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public interface IGetWeatherForecastHandler
{
    Task<Result<GetWeatherForecastResponse>> GetWeatherForecast(GetWeatherForecastQuery? query);
}
