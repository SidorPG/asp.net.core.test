using Api.Middleware;

namespace Api.Extentions;

public static class ExceptionHandlerMiddlewareExtention
{
    public static IApplicationBuilder UseDbLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}