using YourProjectName.Application.Commons.Responses;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed record GetWeatherForecastResponse() : DataResponse<GetWeatherForecastDataResponse> 
{
    public static GetWeatherForecastResponse Create(IEnumerable<Domain.WeatherForecasts.WeatherForecast> forecasts) 
        => Create<GetWeatherForecastResponse>(new GetWeatherForecastDataResponse(forecasts));
};

public sealed record GetWeatherForecastDataResponse(IEnumerable<Domain.WeatherForecasts.WeatherForecast> Forecasts);