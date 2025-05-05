using YourProjectName.Application.Commons.Responses;
using YourProjectName.Domain.WeatherForecast;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed record GetWeatherForecastResponse() : DataResponse<GetWeatherForecastDataResponse> 
{
    public static GetWeatherForecastResponse Create(IEnumerable<WeatherForecastAggregate> forecasts) => Create<GetWeatherForecastResponse>(new GetWeatherForecastDataResponse(forecasts));
};

public sealed record GetWeatherForecastDataResponse(IEnumerable<WeatherForecastAggregate> Forecasts);