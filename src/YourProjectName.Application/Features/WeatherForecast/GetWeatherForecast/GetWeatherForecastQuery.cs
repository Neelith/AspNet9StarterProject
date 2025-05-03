using YourProjectName.Application.Commons.Requests;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed record GetWeatherForecastQuery(
    int? TemperatureRangeMin,
    int? TemperatureRangeMax)
    : IQuery;
