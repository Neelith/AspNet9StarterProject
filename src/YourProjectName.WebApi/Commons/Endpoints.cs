using Carter;

namespace YourProjectName.WebApi.Commons;

public abstract class Endpoints : ICarterModule
{
    public abstract void AddRoutes(IEndpointRouteBuilder app);
}
