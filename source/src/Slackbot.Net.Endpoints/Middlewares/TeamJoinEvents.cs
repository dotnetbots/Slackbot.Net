using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

public class TeamJoinEvents(
    RequestDelegate next,
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
            try
            {
                logger.LogInformation("Handling using {HandlerType}", handler.GetType());
                var response = await handler.Handle(metadata, teamJoinEvent);
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
            && ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.TeamJoin;
    }
}
