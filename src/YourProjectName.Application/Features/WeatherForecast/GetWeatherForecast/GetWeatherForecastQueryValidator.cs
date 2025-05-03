using FluentValidation;

namespace YourProjectName.Application.Features.WeatherForecast.GetWeatherForecast;

public sealed class GetWeatherForecastQueryValidator : AbstractValidator<GetWeatherForecastQuery>
{
    public GetWeatherForecastQueryValidator()
    {
        RuleFor(x => x.TemperatureRangeMin)
            .GreaterThanOrEqualTo(-20)
            .LessThanOrEqualTo(55)
            .When(x => x.TemperatureRangeMin.HasValue);

        RuleFor(x => x.TemperatureRangeMax)
            .GreaterThanOrEqualTo(-20)
            .LessThanOrEqualTo(55)
            .When(x => x.TemperatureRangeMax.HasValue);
    }
}
