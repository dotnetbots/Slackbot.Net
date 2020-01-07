using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Interactive;

namespace Slackbot.Net.Endpoints.Hosting
{
    internal class HttpResponderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpResponderMiddleware> _logger;
        private readonly IHandleInteractiveActions _responseHandler;

        public HttpResponderMiddleware(RequestDelegate next, ILogger<HttpResponderMiddleware> logger, IHandleInteractiveActions responseHandler)
        {
            _next = next;
            _logger = logger;
            _responseHandler = responseHandler;
        }

        public async Task Invoke(HttpContext context)
        {

            var body = await context.Request.ReadFormAsync();
            _logger.LogInformation(JsonConvert.SerializeObject(body));
            var payload = body["payload"];
            if (string.IsNullOrEmpty(payload))
            {
                _logger.LogError("No payload");
                await context.WriteJsonResponse(400, "{ \"error\" : \"No/empty payload\" }");
            }
            else
            {
                IncomingInteractiveMessage incomingInteractiveMessage = null;
                try
                {
                    incomingInteractiveMessage = JsonConvert.DeserializeObject<IncomingInteractiveMessage>(payload);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString());
                }

                if (incomingInteractiveMessage != null && incomingInteractiveMessage.HasValues())
                {
                    var handleResponse = await _responseHandler.RespondToSlackInteractivePayload(incomingInteractiveMessage);
                    var serializeObject = JsonConvert.SerializeObject(handleResponse);
                    _logger.LogInformation(serializeObject);
                    await context.WriteJsonResponse(200, serializeObject);
                }
                else
                {
                    _logger.LogError("Invalid payload");
                    _logger.LogError($"payload={payload}");
                    await context.WriteJsonResponse(400, $"{{ \"error\" : \"Invalid payload\", \"payload\" : {payload} }}");
                }
                // including the next middleware, even when no middlewares are present
                // logs exceptions, but returns as expected.
                // Some internal Kestrel stuff is writing to the stream here..
                // commenting out to avoid it
                //await _next(context);
            }
        }
    }
}