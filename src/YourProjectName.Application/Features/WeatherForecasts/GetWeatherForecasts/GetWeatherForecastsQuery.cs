using YourProjectName.Application.Commons.Requests;

namespace YourProjectName.Application.Features.WeatherForecasts.GetWeatherForecasts;

public sealed record GetWeatherForecastsQuery(
    int? TemperatureRangeMin,
    int? TemperatureRangeMax)
    : IQuery;
