using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Interactive.ViewSubmissions;
using Slackbot.Net.Endpoints.Middlewares;

namespace Slackbot.Net.Endpoints.Interactive
{
    internal class Interactive
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Interactive> _logger;
        private readonly IEnumerable<IHandleViewSubmissions> _responseHandlers;
        private NoOpViewSubmissionHandler _noOp;

        public Interactive(RequestDelegate next, ILogger<Interactive> logger, IEnumerable<IHandleViewSubmissions> responseHandlers, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = logger;
            _responseHandlers = responseHandlers;
            _noOp = new NoOpViewSubmissionHandler(loggerFactory.CreateLogger<NoOpViewSubmissionHandler>());
        }

        public async Task Invoke(HttpContext context)
        {
            var payload = (Interaction) context.Items[HttpItemKeys.InteractivePayloadKey];
            var interactiveType = payload.Type;
            
            if (interactiveType == InteractionTypes.ViewSubmission)
            {
                await HandleViewSubmission(payload as ViewSubmission);
            }
            else
            {
                await _noOp.Handle(payload);
            }

            await _next(context);
        }

        private async Task HandleViewSubmission(ViewSubmission payload)
        {
            var handler = _responseHandlers.FirstOrDefault();
            
            if (handler == null)
            {
                _logger.LogError("No handler registered for ViewSubmission interactions");
                await _noOp.Handle(payload);
            }
            else
            {
                _logger.LogInformation($"Handling using {handler.GetType()}");
                try
                {
                    _logger.LogInformation($"Handling using {handler.GetType()}");
                    var response = await handler.Handle(payload);
                    _logger.LogInformation(response.HandledMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }

        public static bool ShouldRun(HttpContext ctx) => ctx.Items.ContainsKey(HttpItemKeys.InteractivePayloadKey);
    }
}