using Microsoft.AspNetCore.Mvc;
using YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;
using YourProjectName.Shared.Results;
using YourProjectName.WebApi.Constants;
using YourProjectName.WebApi.Infrastructure.Extensions;

namespace YourProjectName.WebApi.Endpoints.WeatherForecast;

public class WeatherForecastsEndpoints : IEndpoints
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("weatherforecasts")
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

        group.MapGet("/exception",
            async Task<IResult>
            () =>
            {
                throw new Exception("Test exception");
            })
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

        group.MapGet("/bad",
            async Task<IResult>
            () =>
            {
                var result = Result.Fail(new Error("Test.Error", "A very bad request", ErrorType.Validation));
                return result.ToErrorResponse();
            })
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);

    }
}