using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Authentication;

namespace Slackbot.Net.Endpoints.Middlewares;

internal class SlackbotEventAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SlackbotEventAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx, ILogger<SlackbotEventAuthMiddleware> logger)
    {
        AuthenticateResult res;
        try
        {
            res = await ctx.AuthenticateAsync(SlackbotEventsAuthenticationConstants.AuthenticationScheme);
        }
        catch (InvalidOperationException ioe)
        {
            throw new InvalidOperationException("Did you forget to call services.AddAuthentication().AddSlackbotEvents()?", ioe);
        }

        if (res.Succeeded)
        {
            await _next(ctx);
        }
        else
        {
            logger.LogWarning($"Unauthorized callback from Slack");
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await ctx.Response.WriteAsync("UNAUTHORIZED");
        }
    }
}