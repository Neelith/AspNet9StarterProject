using YourProjectName.Application.Commons.Responses;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed record GetWeatherForecastResponse() : DataResponse<GetWeatherForecastDataResponse>;

public sealed record GetWeatherForecastDataResponse(IEnumerable<Domain.WeatherForecast.WeatherForecast> Forecasts);