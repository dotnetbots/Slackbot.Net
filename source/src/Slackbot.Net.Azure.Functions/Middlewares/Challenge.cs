using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace Slackbot.Net.Azure.Functions.Middlewares;

public class Challenge : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext functionContext, FunctionExecutionDelegate next)
    {
        var logger = functionContext.GetLogger<Challenge>();
        var context = functionContext.GetHttpContext();

        if (!context.Items.ContainsKey(HttpItemKeys.ChallengeKey))
        {
            await next(functionContext);
            return;
        }


        var challenge = context.Items[HttpItemKeys.ChallengeKey];
        logger.LogInformation($"Handling challenge request. Challenge: {challenge}");
        context.Response.StatusCode = 200;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new {challenge}));
    }
}
