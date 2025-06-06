using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Models.Events;
using Slackbot.Net.Endpoints.Models.Interactive;
using Slackbot.Net.Endpoints.Models.Interactive.BlockActions;
using Slackbot.Net.Endpoints.Models.Interactive.MessageActions;
using Slackbot.Net.Endpoints.Models.Interactive.ViewSubmissions;

namespace Slackbot.Net.Endpoints.Middlewares;

public class HttpItemsManager(RequestDelegate next, ILogger<HttpItemsManager> logger)
{
    private static readonly JsonSerializerOptions WebOptions = new(JsonSerializerDefaults.Web);

    public async Task Invoke(HttpContext context)
    {
        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true))
        {
            var body = await reader.ReadToEndAsync();

            if (body.StartsWith("{"))
            {
                var jObject = JsonDocument.Parse(body);
                logger.LogTrace(body);
                JsonElement challengeValue;
                var isChallenge = jObject.RootElement.TryGetProperty("challenge", out challengeValue);
                if (isChallenge)
                {
                    context.Items.Add(HttpItemKeys.ChallengeKey, challengeValue);
                }
                else
                {
                    var metadata = JsonSerializer.Deserialize<EventMetaData>(body, WebOptions);
                    if (jObject.RootElement.GetProperty("event") is JsonElement @event)
                    {
                        var slackEvent = ToEventType(@event, body);
                        context.Items.Add(HttpItemKeys.EventMetadataKey, metadata);
                        context.Items.Add(HttpItemKeys.SlackEventKey, slackEvent);
                        context.Items.Add(HttpItemKeys.EventTypeKey, @event.GetProperty("type"));
                    }
                }
            }
            // https://api.slack.com/interactivity/handling#payloads
            // The body of that request will contain a payload parameter.
            // Your app should parse this payload parameter as JSON.
            else if (body.StartsWith("payload="))
            {
                logger.LogTrace(body);
                var payloadJsonUrlEncoded = body.Remove(0, 8);
                var decodedJson = WebUtility.UrlDecode(payloadJsonUrlEncoded);
                var payload = JsonDocument.Parse(decodedJson).RootElement;
                var interactivePayloadTyped = ToInteractiveType(payload, body);
                context.Items.Add(HttpItemKeys.InteractivePayloadKey, interactivePayloadTyped);
            }

            context.Request.Body.Position = 0;
        }

        await next(context);
    }

    private static SlackEvent ToEventType(JsonElement eventJson, string raw)
    {
        var eventType = GetEventType(eventJson);
        var json = eventJson.ToString();
        switch (eventType)
        {
            case EventTypes.AppMention:
                return JsonSerializer.Deserialize<AppMentionEvent>(json, WebOptions);
            case EventTypes.MemberJoinedChannel:
                return JsonSerializer.Deserialize<MemberJoinedChannelEvent>(json, WebOptions);
            case EventTypes.AppHomeOpened:
                return JsonSerializer.Deserialize<AppHomeOpenedEvent>(json, WebOptions);
            case EventTypes.TeamJoin:
                return JsonSerializer.Deserialize<TeamJoinEvent>(json, WebOptions);
            case EventTypes.EmojiChanged:
                return JsonSerializer.Deserialize<EmojiChangedEvent>(json, WebOptions);            
            default:
                var unknownSlackEvent = JsonSerializer.Deserialize<UnknownSlackEvent>(json, WebOptions);
                unknownSlackEvent.RawJson = raw;
                return unknownSlackEvent;
        }
    }

    private static Interaction ToInteractiveType(JsonElement payloadJson, string raw)
    {
        var eventType = GetEventType(payloadJson);
        var json = payloadJson.ToString();
        switch (eventType)
        {
            case InteractionTypes.ViewSubmission:
                var viewSubmission = JsonSerializer.Deserialize<ViewSubmission>(json, WebOptions);

                var view = payloadJson.GetProperty("view");
                var viewState = view.GetProperty("state");
                viewSubmission.ViewId = view.GetProperty("id").GetString();
                viewSubmission.ViewState = viewState;
                return viewSubmission;
            case InteractionTypes.BlockActions:
                return JsonSerializer.Deserialize<BlockActionInteraction>(json, WebOptions);
            case InteractionTypes.MessageAction:
                return JsonSerializer.Deserialize<MessageActionInteraction>(json, WebOptions);
            default:
                var unknownSlackEvent = JsonSerializer.Deserialize<UnknownInteractiveMessage>(json, WebOptions);
                unknownSlackEvent.RawJson = raw;
                return unknownSlackEvent;
        }
    }

    public static string GetEventType(JsonElement eventJson)
    {
        if (eventJson.ValueKind != JsonValueKind.Null)
        {
            return eventJson.GetProperty("type").GetString();
        }

        return "unknown";
    }
}
