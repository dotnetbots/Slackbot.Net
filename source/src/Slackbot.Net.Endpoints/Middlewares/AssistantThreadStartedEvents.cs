using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

public class AssistantThreadStartedEvents(
    RequestDelegate next,
    ILogger<AssistantThreadStartedEvents> logger,
    IEnumerable<IHandleAssistantThreadStarted> responseHandlers
)
{
    public async Task Invoke(HttpContext context)
    {
        var assistantEvent = (AssistantThreadStartedEvent)context.Items[HttpItemKeys.SlackEventKey];
        var metadata = (EventMetaData)context.Items[HttpItemKeys.EventMetadataKey];

        foreach (var handler in responseHandlers)
        {
            try
            {
                logger.LogInformation("Handling using {HandlerType}", handler.GetType());
                var response = await handler.Handle(metadata, assistantEvent);
                logger.LogInformation("Handler response: {Response}", response.Response);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        context.Response.StatusCode = 200;
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.EventTypeKey)
            && ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.AssistantThreadStarted;
    }
}
