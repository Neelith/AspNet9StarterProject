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
            async Task<Results<
                BadRequest<ProblemDetails>, InternalServerError<ProblemDetails>, 
                Ok<GetWeatherForecastResponse>>> 
            ([AsParameters] GetWeatherForecastQuery query,
            [FromServices] IGetWeatherForecastHandler handler,
            HttpContext context) =>
            {
                var result = await handler.GetWeatherForecast(query);

                var instance = context.Request.Path;
                var errorCode = result.Errors.GetResultErrorCode();
                //TODO
                errorCode switch
                {
                    Errors.ValidationErrorCode => result.ToBadRequest(instance),
                    Errors.NotFoundErrorCode => result.ToNotFound(instance),
                    Errors.InternalErrorCode => result.ToInternalServerError(instance),
                    _ => throw new Exception("Unknown error")
                };

                return result.IsSuccess ? TypedResults.Ok(result.Value) : result.ToProblem();
            })
            .WithName("GetWeatherForecast");
    }
}
