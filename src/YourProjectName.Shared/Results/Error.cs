namespace YourProjectName.Shared.Results;

public record Error
{
    public const string NullErrorCode = "Null";
    public const string ValidationErrorCode = "Validation";
    public const string NotFoundErrorCode = "NotFound";
    public const string InternalErrorCode = "InternalError";

    public string Code { get; }

    public string Description { get; }

    public ErrorType Type { get; }

    public Error(string code, string description, ErrorType type)
    {
        Code = code;
        Description = description;
        Type = type;
    }

    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static readonly Error NullValue = new(
        NullErrorCode,
        "Null value was provided",
        ErrorType.Failure);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error Problem(string code, string description) =>
        new(code, description, ErrorType.Problem);

    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);
}
