using Api.Exceptions;
using Data;
using Data.Models;
  
namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILoggerProvider loggerProvider)
    {
        _next = next;
        _logger = loggerProvider.CreateLogger("ExceptionHandlingMiddleware");
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
    {
        try
        {
            try { context.Request.EnableBuffering(); } catch { }
            await _next(context);
        }
        catch (Exception exception)
        {
            var messageType = exception is SecureException ? "Secure" : exception.GetType().FullName;

            int.TryParse(context.Items["X-Request-ID"]?.ToString(), out var requestId);
            requestId++;
            var journal_event = new journal_event()
            {
                Id = requestId,
                Created = DateTime.UtcNow,
                Path = $"{context.Request.Path}",
                RequestQuery = getQuery(context),
                RequestBody = getBody(context),
                Exception = exception.GetType().FullName,
                ExceptionMessage = exception.Message,
                ExceptionStackTrace = exception.StackTrace,
                Message = new journal_message()
                {
                    EventId = requestId,
                    Type = messageType,
                }
            };
            await dbContext.AddAsync(journal_event);
            await dbContext.SaveChangesAsync();

            var problemDetails = new
            {
                type = messageType,
                id = requestId,
                data = new { message = exception is SecureException se ? $"{se.Message}" : $"Internal server error ID = {requestId}." },
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }

    private static string getQuery(HttpContext context)
    {
        var data = new List<string>();
        foreach (var item in context.Request.Query.Keys)
        {
            data.Add($"{item} = {context.Request.Query[item]}");
        }
        return string.Join("\r\n", data.ToArray());
    }
    private static string getBody(HttpContext context)
    {
        try
        {
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using StreamReader stream = new(context.Request.Body);
            string body = stream.ReadToEnd();
            return body;
        }
        catch { }
        return "";
    }
}
