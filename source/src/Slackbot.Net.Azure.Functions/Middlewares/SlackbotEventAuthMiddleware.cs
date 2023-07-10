using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Slackbot.Net.Azure.Functions.Authentication;

namespace Slackbot.Net.Azure.Functions.Middlewares;

internal class SlackbotEventAuthMiddleware : IFunctionsWorkerMiddleware
{


    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        bool success = false;
        var ctx = context.GetHttpContext();
        try
        {
            var res = await ctx.AuthenticateAsync(SlackbotEventsAuthenticationConstants.AuthenticationScheme);
            success = res.Succeeded;
        }
        catch (InvalidOperationException ioe)
        {
            throw new InvalidOperationException("Did you forget to call services.AddAuthentication().AddSlackbotEvents()?", ioe);
        }

        if (success)
        {
            await next(context);
        }
        else
        {
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await ctx.Response.WriteAsync("UNAUTHORIZED");
        }
    }
}
