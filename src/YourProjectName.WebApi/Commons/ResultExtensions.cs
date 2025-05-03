using FluentResults;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YourProjectName.Domain.Commons;

namespace YourProjectName.WebApi.Commons;

public static class ResultExtensions
{
    public static IResult Match<T>(
        this Result<T> result,
        Func<Result<T>, IResult> onSuccess,
        Func<Result<T>, IResult> onFailure)
    {
        if (result.IsFailed)
        {
            return onFailure(result);
        }

        return onSuccess(result);
    }

    public static IResult ToErrorResponse<T>(
        this Result<T> result,
        string? instance = default)
    {
        if (result is null || result.IsSuccess)
        {
            throw new ArgumentException("Expected 'failed' result, but 'success' result was found instead");
        }

        var errorCode = result.Errors.GetResultErrorCode();

        return errorCode switch
        {
            Errors.ValidationErrorCode => result.ToBadRequest(instance),
            Errors.NotFoundErrorCode => result.ToNotFound(instance),
            Errors.InternalErrorCode => result.ToInternalServerError(instance),
            _ => throw new ArgumentException("Unhandled result error code"),
        };
    }

    public static BadRequest<ProblemDetails> ToBadRequest<T>(this Result<T> result, string? instance)
    {
        ThrowIfErrorResultIsNotValid(result.ToResult(), Errors.ValidationErrorCode);

        string[] errors = result.Errors.GetArrayOfErrorMessages();

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors found",
            Detail = errors is not null && errors.Length != 0 ? string.Join($"{Environment.NewLine}- ", errors) : null,
            Instance = instance
        };

        return TypedResults.BadRequest(problem);
    }

    public static NotFound<ProblemDetails> ToNotFound<T>(this Result<T> result, string? instance)
    {
        ThrowIfErrorResultIsNotValid(result.ToResult(), Errors.NotFoundErrorCode);

        string[] errors = result.Errors.GetArrayOfErrorMessages();

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            Title = "The resource was not found",
            Detail = errors is not null && errors.Length != 0 ? string.Join($"{Environment.NewLine}- ", errors) : null,
            Instance = instance
        };

        return TypedResults.NotFound(problem);
    }

    public static InternalServerError<ProblemDetails> ToInternalServerError<T>(this Result<T> result, string? instance)
    {
        ThrowIfErrorResultIsNotValid(result.ToResult(), Errors.InternalErrorCode);

        string[] errors = result.Errors.GetArrayOfErrorMessages();

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Internal server error",
            Detail = errors is not null && errors.Length != 0 ? string.Join($"{Environment.NewLine}- ", errors) : null,
            Instance = instance
        };

        return TypedResults.InternalServerError(problem);
    }

    public static int? GetResultErrorCode(this IEnumerable<IError> errors)
    {
        if (errors is null || !errors.Any())
        {
            return null;
        }

        return (int?)errors?.FirstOrDefault()?.Metadata.FirstOrDefault(m => m.Key == Errors.ErrorCode).Value;
    }

    private static string[] GetArrayOfErrorMessages(this IEnumerable<IError> errors)
    {
        return [.. errors.Select(e => e.Message)];
    }

    private static void ThrowIfErrorResultIsNotValid(Result result, int errorCode)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsSuccess)
        {
            throw new ArgumentException("Expected 'failed' result, but 'success' result was found instead");
        }

        var code = result.Errors.GetResultErrorCode();

        if (!code.HasValue || code != errorCode)
        {
            throw new ArgumentException($"Expected '{Errors.ErrorCode}' to be '{errorCode}' but '{code} was found instead'");
        }
    }
}
