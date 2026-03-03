using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

internal class MemberJoinedEvents(
    RequestDelegate next,
    ILogger<MemberJoinedEvents> logger,
    IEnumerable<IHandleMemberJoinedChannel> responseHandlers
)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var memberJoinedChannelEvent = (MemberJoinedChannelEvent)
            context.Items[HttpItemKeys.SlackEventKey];
        var metadata = (EventMetaData)context.Items[HttpItemKeys.EventMetadataKey];

        if (responseHandlers == null)
        {
            logger.LogError("No handler registered for IHandleMemberJoinedChannelEvents");
            return;
        }
        foreach (var handler in responseHandlers)
        {
            logger.LogInformation($"Handling using {handler.GetType()}");
            try
            {
                logger.LogInformation($"Handling using {handler.GetType()}");
                var response = await handler.Handle(metadata, memberJoinedChannelEvent);
                logger.LogInformation(response.Response);
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
            && ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.MemberJoinedChannel;
    }
}
