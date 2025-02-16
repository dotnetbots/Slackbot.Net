using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

internal class AppHomeOpenedEvents(
    RequestDelegate next,
    ILogger<AppHomeOpenedEvents> logger,
    IEnumerable<IHandleAppHomeOpened> responseHandlers)
{
    public async Task Invoke(HttpContext context)
    {
        var appHomeOpenedEvent = (AppHomeOpenedEvent)context.Items[HttpItemKeys.SlackEventKey];
        var metadata = (EventMetaData)context.Items[HttpItemKeys.EventMetadataKey];
        var handler = responseHandlers.FirstOrDefault();

        if (handler == null)
        {
            logger.LogError("No handler registered for IHandleAppHomeOpened");
        }
        else
        {
            logger.LogInformation($"Handling using {handler.GetType()}");
            try
            {
                logger.LogInformation($"Handling using {handler.GetType()}");
                var response = await handler.Handle(metadata, appHomeOpenedEvent);
                logger.LogInformation(response.Response);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        await next(context);
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.EventTypeKey) &&
               ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.AppHomeOpened;
    }
}
