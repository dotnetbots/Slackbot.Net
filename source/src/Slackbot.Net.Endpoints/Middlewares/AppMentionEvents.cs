using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints.Middlewares
{
    public class AppMentionEvents
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppMentionEvents> _logger;
        
        private readonly ISelectEventHandlers _responseHandler;

        public AppMentionEvents(RequestDelegate next, ILogger<AppMentionEvents> logger, ISelectEventHandlers responseHandler)
        {
            _next = next;
            _logger = logger;
            _responseHandler = responseHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            var metadata = (EventMetaData) context.Items[HttpItemKeys.EventMetadataKey];
            var slackEvent = (SlackEvent) context.Items[HttpItemKeys.SlackEventKey];

            if (slackEvent is AppMentionEvent appMentionEvent)
            {
                var handlers = await _responseHandler.GetAppMentionEventHandlerFor(metadata, appMentionEvent);

                foreach (var handler in handlers)
                {
                    try
                    {
                        _logger.LogInformation($"Handling using {handler.GetType()}");
                        await handler.Handle(metadata, appMentionEvent);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                } 
            }

            context.Response.StatusCode = 200;
        }

        public static bool ShouldRun(HttpContext ctx)
        {
            return ctx.Items.ContainsKey(HttpItemKeys.EventTypeKey) && (ctx.Items[HttpItemKeys.EventTypeKey].ToString() == EventTypes.AppMention);
        }
    }
}