using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Hosting;

namespace Slackbot.Net.Endpoints.Generic
{
    public class SlackEventsMiddleware
    {
        private readonly ILogger<SlackEventsMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHandleAllEvents _responseHandler;

        public SlackEventsMiddleware(RequestDelegate next, ILogger<SlackEventsMiddleware> logger, IHandleAllEvents responseHandler)
        {
            _next = next;
            _logger = logger;
            _responseHandler = responseHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            IFormCollection body = await context.Request.ReadFormAsync();
            _logger.LogInformation(JsonConvert.SerializeObject(body));

            string challenge = body["challenge"];
            string @event = body["event"];
            if (!string.IsNullOrEmpty(challenge))
            {
                await context.WriteJsonResponse(200, JsonConvert.SerializeObject(new {challenge}));
            }
            else if (!string.IsNullOrEmpty(@event))
            {
                context.Response.StatusCode = 200;
                await _responseHandler.Handle(@event);
            }
            else
            {
                _logger.LogError("No payload");
                await context.WriteJsonResponse(400, "{ \"error\" : \"No/empty payload\" }");
            }
        }
    }
}



