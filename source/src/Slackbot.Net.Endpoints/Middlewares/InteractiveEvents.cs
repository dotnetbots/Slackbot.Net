using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Interactive;
using Slackbot.Net.Endpoints.Models.Interactive.BlockActions;
using Slackbot.Net.Endpoints.Models.Interactive.MessageActions;
using Slackbot.Net.Endpoints.Models.Interactive.ViewSubmissions;

namespace Slackbot.Net.Endpoints.Middlewares;

internal class InteractiveEvents(
    RequestDelegate next,
    ILogger<InteractiveEvents> logger,
    IEnumerable<IHandleViewSubmissions> responseHandlers,
    IEnumerable<IHandleInteractiveBlockActions> blockActionHandlers,
    IEnumerable<IHandleMessageActions> messageActionHandlers,
    ILoggerFactory loggerFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly NoOpViewSubmissionHandler _noOp = new(loggerFactory.CreateLogger<NoOpViewSubmissionHandler>());

    public async Task Invoke(HttpContext context)
    {
        var payload = (Interaction)context.Items[HttpItemKeys.InteractivePayloadKey];

        switch (payload.Type)
        {
            case InteractionTypes.ViewSubmission:
                await HandleViewSubmission(payload as ViewSubmission);
                break;
            case InteractionTypes.BlockActions:
                var res = await HandleBlockActions(payload as BlockActionInteraction);
                context.Response.StatusCode = res.Response switch
                {
                    "ERROR" => 500,
                    "VALIDATION_ERRORS" => 400,
                    _ => context.Response.StatusCode
                };
                await context.Response.WriteAsync(res.Response);
                break;
            case InteractionTypes.MessageAction:
                await HandleMessageAction(payload as MessageActionInteraction);
                break;
            default:
                await _noOp.Handle(payload);
                break;
        }
    }

    private async Task HandleMessageAction(MessageActionInteraction messageAction)
    {
        var handler = messageActionHandlers.FirstOrDefault();

        if (handler == null)
        {
            logger.LogError($"No handler registered for {nameof(MessageActionInteraction)} interactions");
            await _noOp.Handle(messageAction);
        }
        else
        {
            logger.LogInformation($"Handling using {handler.GetType()}");
            var response = await handler.Handle(messageAction);
            logger.LogInformation(response.Response);
        }
    }

    private async Task<EventHandledResponse> HandleBlockActions(BlockActionInteraction payload)
    {
        var handler = blockActionHandlers.FirstOrDefault();

        if (handler == null)
        {
            logger.LogError("No handler registered for BlockAction interactions");
            return await _noOp.Handle(payload);
        }

        try
        {
            logger.LogInformation($"Handling using {handler.GetType()}");
            var response = await handler.Handle(payload);
            logger.LogInformation(response.Response);
            return response;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return new EventHandledResponse("ERROR");
        }
    }

    private async Task HandleViewSubmission(ViewSubmission payload)
    {
        var handler = responseHandlers.FirstOrDefault();

        if (handler == null)
        {
            logger.LogError("No handler registered for ViewSubmission interactions");
            await _noOp.Handle(payload);
        }
        else
        {
            logger.LogInformation($"Handling using {handler.GetType()}");
            try
            {
                logger.LogInformation($"Handling using {handler.GetType()}");
                var response = await handler.Handle(payload);
                logger.LogInformation(response.Response);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }
    }

    public static bool ShouldRun(HttpContext ctx)
    {
        return ctx.Items.ContainsKey(HttpItemKeys.InteractivePayloadKey);
    }
}
