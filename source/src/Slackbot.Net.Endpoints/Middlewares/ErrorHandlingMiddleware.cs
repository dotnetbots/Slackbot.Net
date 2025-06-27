using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Slackbot.Net.Endpoints.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<HttpItemsManager> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            context.Response.Headers.Append("x-slack-no-retry", "1");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred while processing the slack event request. Plz do not retry.");
        }
    }
}
