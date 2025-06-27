using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

public class TeamJoinEvents(
#pragma warning disable CS9113 // Parameter is unread.
    RequestDelegate next,
#pragma warning restore CS9113 // Parameter is unread.
    ILogger<TeamJoinEvents> logger,
    IEnumerable<IHandleTeamJoin> responseHandlers
)
{
    public async Task Invoke(HttpContext context)
    {
        var teamJoinEvent = (TeamJoinEvent)context.Items[HttpItemKeys.SlackEventKey];
        var metadata = (EventMetaData)context.Items[HttpItemKeys.EventMetadataKey];
        var handler = responseHandlers.FirstOrDefault();

        if (handler == null)
        {
            logger.LogError("No handler registered for IHandleTeamJoinEvents");
        }
        else
        {
            logger.LogInformation("Handling using {HandlerType}", handler.GetType());
            var response = await handler.Handle(metadata, teamJoinEvent);
            logger.LogInformation("Handler response: {Response}", response.Response);
        }

        context.Response.StatusCode = 200;
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.EventTypeKey)
            && ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.TeamJoin;
    }
}
