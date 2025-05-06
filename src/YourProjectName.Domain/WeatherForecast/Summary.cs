using FluentResults;
using YourProjectName.Domain.Commons;

namespace YourProjectName.Domain.WeatherForecast;
public record Summary
{
    public string Value { get; private set; }

    private Summary(string value)
    {
        Value = value;
    }

    public static Result<Summary> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Fail(Errors.ValidationError("Summary cannot be empty"));
        }

        if (value.Length > 256)
        {
            return Result.Fail(Errors.ValidationError("Summary cannot exceed 256 characters"));
        }

        return Result.Ok(new Summary(value));
    }
}
