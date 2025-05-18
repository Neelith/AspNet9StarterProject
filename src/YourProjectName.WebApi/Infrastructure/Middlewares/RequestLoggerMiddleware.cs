
using Microsoft.Extensions.Primitives;
using Serilog.Context;
using YourProjectName.WebApi.Constants;

namespace YourProjectName.WebApi.Infrastructure.Middlewares;

public class RequestLoggerMiddleware(RequestDelegate next)
{
    public Task InvokeAsync(HttpContext context)
    {
        bool isTraceHeaderPresent = context.Request.Headers.TryGetValue(Headers.Trace, out StringValues strings);

        string traceId = isTraceHeaderPresent ? strings.ToString() : context.TraceIdentifier;

        using (LogContext.PushProperty("TraceId", traceId))
        {
            return next(context);
        }
    }
}
