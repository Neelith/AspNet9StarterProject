using Microsoft.AspNetCore.Mvc;
using YourProjectName.Application.WeatherForecast.GetWeatherForecast;
using YourProjectName.WebApi.Commons;
using IGetWeatherForecastHandler = YourProjectName.Application.WeatherForecast.GetWeatherForecast.IGetWeatherForecastHandler;

namespace YourProjectName.WebApi.WeatherForecast
{
    public class GetWeatherForecast : IEndpoint
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/weatherforecast", async (
                [AsParameters] GetWeatherForecastQuery query,
                [FromServices] IGetWeatherForecastHandler handler) => 
                {
                    var response = await handler.GetWeatherForecast(query);
                    return TypedResults.Ok(response);
                })
               .WithName("GetWeatherForecast");
        }
    }
}
