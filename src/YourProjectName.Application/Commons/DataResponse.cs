namespace YourProjectName.Application.Commons;

public abstract class DataResponse<TData> : IResponse where TData : notnull
{
    public required TData Data { get; init; }
}
