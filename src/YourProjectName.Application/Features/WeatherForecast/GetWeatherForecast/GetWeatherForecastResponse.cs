using YourProjectName.Application.Commons.Responses;
using YourProjectName.Domain.WeatherForecast;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed record GetWeatherForecastResponse() : DataResponse<GetWeatherForecastDataResponse>;

public sealed record GetWeatherForecastDataResponse(IEnumerable<WeatherForecastAggregate> Forecasts);