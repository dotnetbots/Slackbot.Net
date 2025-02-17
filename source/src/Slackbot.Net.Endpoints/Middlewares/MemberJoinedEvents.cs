using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares;

internal class MemberJoinedEvents
{
    private readonly ILogger<MemberJoinedEvents> _logger;
    private readonly RequestDelegate _next;
    private readonly IEnumerable<IHandleMemberJoinedChannel> _responseHandlers;

    public MemberJoinedEvents(RequestDelegate next, ILogger<MemberJoinedEvents> logger,
        IEnumerable<IHandleMemberJoinedChannel> responseHandlers, ILoggerFactory loggerFactory)
    {
        _next = next;
        _logger = logger;
        _responseHandlers = responseHandlers;
    }

    public async Task Invoke(HttpContext context)
    {
        var memberJoinedChannelEvent = (MemberJoinedChannelEvent)context.Items[HttpItemKeys.SlackEventKey];
        var metadata = (EventMetaData)context.Items[HttpItemKeys.EventMetadataKey];
        var handler = _responseHandlers.FirstOrDefault();

        if (handler == null)
        {
            _logger.LogError("No handler registered for IHandleMemberJoinedChannelEvents");
        }
        else
        {
            _logger.LogInformation($"Handling using {handler.GetType()}");
            try
            {
                _logger.LogInformation($"Handling using {handler.GetType()}");
                var response = await handler.Handle(metadata, memberJoinedChannelEvent);
                _logger.LogInformation(response.Response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        context.Response.StatusCode = 200;
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.EventTypeKey) &&
               ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.MemberJoinedChannel;
    }
}
