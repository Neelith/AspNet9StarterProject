using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;
using YourProjectName.Domain.Commons;
using YourProjectName.WebApi.Commons;
using YourProjectName.WebApi.Constants;
using IGetWeatherForecastHandler = YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast.IGetWeatherForecastHandler;

namespace YourProjectName.WebApi.Endpoints.WeatherForecast;

public class WeatherForecastEndpoints : IEndpoints
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("weatherforecast")
            .WithTags(Tags.WeatherForecast)
            .WithDescription("Weather forecast endpoints");

        group.MapGet("/",
            async Task<IResult>
            ([AsParameters] GetWeatherForecastQuery query,
            [FromServices] IGetWeatherForecastHandler handler,
            HttpContext context) =>
            {
                var result = await handler.GetWeatherForecast(query);

                return result.Match(
                    result => TypedResults.Ok(result.Value),
                    result => result.ToErrorResponse(context.Request.Path)
                );
            })
            .WithName("GetWeatherForecast")
            .Produces<GetWeatherForecastResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}
