using YourProjectName.Application.Commons.Requests;

namespace YourProjectName.Application.Commons.Handlers;

public interface IHandler<TRequest, TResponse>
    where TRequest : IRequest
{
    Task<TResponse> HandleAsync(TRequest request);
}
