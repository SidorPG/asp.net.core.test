using Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Middleware;

public class RequestIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<RequestIdMiddleware> logger,
        ApplicationDbContext dbContext
    )
    {
        var requestId = (await dbContext.JournalEvents.MaxAsync(x => (int?)x.Id)) ?? 0;
        using (logger.BeginScope(new Dictionary<string, object> { ["X-Request-ID"] = requestId }))
        {
            context.Items["X-Request-ID"] = requestId;
            await next(context);
        }
    }
}