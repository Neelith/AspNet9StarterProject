using YourProjectName.Application.Commons.Requests;
using YourProjectName.Application.Commons.Responses;

namespace YourProjectName.Application.Commons.Handlers;

public interface IHandler<in TRequest, TResponse>
    where TRequest : IRequest
{
    Task<TResponse> HandleAsync(TRequest request);
}
