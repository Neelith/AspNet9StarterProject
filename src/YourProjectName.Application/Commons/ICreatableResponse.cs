namespace YourProjectName.Application.Commons;

public interface ICreatableResponse<TResponse, TData>
    where TResponse : IResponse
    where TData : DataResponse<TData>
{
    TResponse Create(TData data);
}
