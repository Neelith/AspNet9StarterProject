using FluentResults;

namespace YourProjectName.Domain.Commons;

public static class Errors
{
    public static string ErrorCode => "Code";
    public static ValidationError ValidationError(string message) => new(message);
    public const int ValidationErrorCode = 400;
    public static NotFoundError NotFound(string message) => new(message);
    public const int NotFoundErrorCode = 404;
    public static InternalError InternalError(string message) => new(message);
    public const int InternalErrorCode = 500;
}

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
        Metadata.Add(Errors.ErrorCode, Errors.NotFoundErrorCode);
    }
}

public class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
        Metadata.Add(Errors.ErrorCode, Errors.ValidationErrorCode);
    }
}

public class InternalError : Error
{
    public InternalError(string message) : base(message)
    {
        Metadata.Add(Errors.ErrorCode, Errors.InternalErrorCode);
    }
}