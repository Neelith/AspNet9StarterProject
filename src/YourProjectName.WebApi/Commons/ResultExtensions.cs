using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using YourProjectName.Shared.Results;

namespace YourProjectName.WebApi.Commons;

public static class ResultExtensions
{
    public static IResult Match<T>(
        this Result<T> result,
        Func<Result<T>, IResult> onSuccess,
        Func<Result<T>, IResult> onFailure)
    {
        if (result.IsFailure)
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

        return result.Error.Code switch
        {
            Error.ValidationErrorCode => result.ToBadRequest(instance),
            Error.NotFoundErrorCode => result.ToNotFound(instance),
            Error.InternalErrorCode => result.ToInternalServerError(instance),
            _ => throw new ArgumentException("Unhandled result error code"),
        };
    }

    public static BadRequest<ProblemDetails> ToBadRequest<T>(this Result<T> result, string? instance)
    {
        ThrowIfErrorResultIsNotValid(result, Error.ValidationErrorCode);

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors found",
            Detail = !string.IsNullOrEmpty(result.Error.Description) ? result.Error.Description : null,
            Instance = instance
        };

        return TypedResults.BadRequest(problem);
    }

    public static NotFound<ProblemDetails> ToNotFound<T>(this Result<T> result, string? instance)
    {
        ThrowIfErrorResultIsNotValid(result, Error.NotFoundErrorCode);

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            Title = "The resource was not found",
            Detail = !string.IsNullOrEmpty(result.Error.Description) ? result.Error.Description : null,
            Instance = instance
        };

        return TypedResults.NotFound(problem);
    }

    public static InternalServerError<ProblemDetails> ToInternalServerError<T>(this Result<T> result, string? instance)
    {
        ThrowIfErrorResultIsNotValid(result, Error.InternalErrorCode);

        var problem = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Internal server error",
            Detail = !string.IsNullOrEmpty(result.Error.Description) ? result.Error.Description : null,
            Instance = instance
        };

        return TypedResults.InternalServerError(problem);
    }

    private static void ThrowIfErrorResultIsNotValid(Result result, string errorCode)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result.IsSuccess)
        {
            throw new ArgumentException("Expected 'failed' result, but 'success' result was found instead");
        }

        var code = result.Error.Code;

        if (string.IsNullOrEmpty(code) || code != errorCode)
        {
            throw new ArgumentException($"Expected '{errorCode}' but '{code} was found instead'");
        }
    }
}
