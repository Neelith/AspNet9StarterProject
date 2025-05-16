using Microsoft.AspNetCore.Mvc;
using YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;
using YourProjectName.Shared.Results;
using YourProjectName.WebApi.Commons;
using YourProjectName.WebApi.Constants;

namespace YourProjectName.WebApi.Endpoints.WeatherForecast;

public class WeatherForecastEndpoints : IEndpoints
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("weatherforecast")
            .WithTags(Tags.WeatherForecast)
            .WithDescription("Weather forecast endpoints");

        group.MapGet("/", async
            ([AsParameters] GetWeatherForecastQuery query,
            [FromServices] IGetWeatherForecastHandler handler) =>
            {
                var result = await handler.HandleAsync(query);

                return result.Match(
                    result => TypedResults.Ok(result.Value),
                    result => result.ToErrorResponse()
                );
            })
            .Produces<GetWeatherForecastResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        group.MapGet("/bad",
            async Task<IResult>
            () =>
            {
                throw new Exception("ciaooo");
            })
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapPost("/bad",
            async Task<IResult>
            (CreateWeatherForecastCommand payload) =>
            {
                var result = Result.Fail<CreateWeatherForecastCommand>(new Error("Test.Error", "A very bad request", ErrorType.Validation));
                return result.ToErrorResponse();
            })
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

    }
}

public class CreateWeatherForecastCommand
{
    public int? TemperatureRangeMin { get; set; }
    public int? TemperatureRangeMax { get; set; }
}