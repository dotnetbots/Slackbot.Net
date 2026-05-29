using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

public class ReactionAddedEvents(
    RequestDelegate next,
    ILogger<ReactionAddedEvents> logger,
    IEnumerable<IHandleReactionAdded> responseHandlers
)
{
    public async Task Invoke(HttpContext context)
    {
        var reactionAdded = (ReactionAddedEvent)context.Items[HttpItemKeys.SlackEventKey];
        var metadata = (EventMetaData)context.Items[HttpItemKeys.EventMetadataKey];
        var handlers = responseHandlers.Where(h => h.ShouldHandle(reactionAdded));

        logger.BeginScope(new Dictionary<string, object>
        {
            ["Slack_TeamId"] = metadata?.Team_Id,
            ["Slack_User"] = reactionAdded?.User
        });

        foreach (var handler in handlers)
        {
            try
            {
                logger.LogInformation("Handling using {HandlerType}", handler.GetType());
                var response = await handler.Handle(metadata, reactionAdded);
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
               && ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.ReactionAdded;
    }
}
