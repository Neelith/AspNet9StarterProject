namespace YourProjectName.Application.Commons.Responses;

public abstract record DataResponse<TData>() : IResponse where TData : notnull
{
    public required TData Data { get; init; }
}
