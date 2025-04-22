using Microsoft.AspNetCore.Mvc;
using YourProjectName.Application.WeatherForecast.GetWeatherForecast;
using YourProjectName.WebApi.Commons;
using IGetWeatherForecastHandler = YourProjectName.Application.WeatherForecast.GetWeatherForecast.IGetWeatherForecastHandler;

namespace YourProjectName.WebApi.WeatherForecast
{
    public class WeatherForecastEndpoints : Endpoints
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/weatherforecast", async (
                [AsParameters] GetWeatherForecastQuery query,
                [FromServices] IGetWeatherForecastHandler handler) =>
                {
                    //use Endpoints base class to manage a Result which in turn can be used to manage the response HTTP status code, use also the Problem pattern
                    var response = await handler.GetWeatherForecast(query);
                    return TypedResults.Ok(response);
                })
                .WithName("GetWeatherForecast");
        }
    }
}
