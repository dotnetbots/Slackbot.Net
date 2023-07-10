using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Azure.Functions.Abstractions;
using Slackbot.Net.Azure.Functions.Models.Events;

namespace Slackbot.Net.Azure.Functions.Middlewares;

public class AppMentionEvents : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext functionContext, FunctionExecutionDelegate next)
    {
        var httpContext = functionContext.GetHttpContext();

        if (!(httpContext.Items.ContainsKey(HttpItemKeys.EventTypeKey) &&
              (httpContext.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.AppMention)))
        {
            await next(functionContext);
            return;
        }


        var metadata = (EventMetaData) httpContext.Items[HttpItemKeys.EventMetadataKey];
        var appMentionEvent = (AppMentionEvent) httpContext.Items[HttpItemKeys.SlackEventKey];
        var responseHandler = functionContext.InstanceServices.GetRequiredService<ISelectAppMentionEventHandlers>() ?? throw new ArgumentNullException("functionContext.InstanceServices.GetRequiredService<ISelectAppMentionEventHandlers>()");
        var handlers = await responseHandler.GetAppMentionEventHandlerFor(metadata, appMentionEvent);
        var logger = functionContext.GetLogger<AppMentionEvents>();
        logger.BeginScope(new Dictionary<string, object>
        {
            ["Slack_TeamId"] = metadata?.Team_Id,
            ["Slack_Channel"] = appMentionEvent?.Channel,
            ["Slack_User"] = appMentionEvent?.User,
        });
        foreach (var handler in handlers)
        {
            try
            {
                logger.LogInformation($"Handling using {handler.GetType()}");
                await handler.Handle(metadata, appMentionEvent);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        httpContext.Response.StatusCode = 200;
    }
}
