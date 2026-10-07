using System.Net;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.Models.Interactive.BlockActions;

namespace Slackbot.Net.Tests;

public class BlockActionsPayloadTests
{
    private const string BlockActionsJson = """
        {
            "type": "block_actions",
            "team": { "id": "T9TK3CUKW", "domain": "example" },
            "user": { "id": "UA8RXUSPL", "username": "jtorrance", "team_id": "T9TK3CUKW" },
            "api_app_id": "AABA1ABCD",
            "token": "9s8d9as89d8as9d8as989",
            "trigger_id": "12321423423.333649436676.d8c1bb837935619ccad0f624c448ffb3",
            "channel": { "id": "CBR2V3XEX", "name": "review-updates" },
            "message": { "bot_id": "BAH5CA16Z", "type": "message", "text": "This content can't be displayed.", "user": "UAJ2RU415", "ts": "1548261231.000200" },
            "response_url": "https://hooks.slack.com/actions/AABA1ABCD/1232321423432/D09sSasdasdAS9091209",
            "actions": [
                {
                    "action_id": "WaXA",
                    "block_id": "=qXel",
                    "text": { "type": "plain_text", "text": "View", "emoji": true },
                    "value": "click_me_123",
                    "type": "button",
                    "action_ts": "1548426417.840180"
                }
            ]
        }
        """;

    [Fact]
    public async Task BlockActionHandlers_ReceiveTheResponseUrl()
    {
        var handler = await Post(BlockActionsJson);

        var received = Assert.Single(handler.Received);
        Assert.Equal("https://hooks.slack.com/actions/AABA1ABCD/1232321423432/D09sSasdasdAS9091209", received.Response_Url);
        Assert.Equal("UA8RXUSPL", received.User.User_Id);
        Assert.Equal("CBR2V3XEX", received.Channel.Id);
    }

    private static async Task<RecordingBlockActionsHandler> Post(string payloadJson)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSlackBotEvents().AddInteractiveBlockActionsHandler<RecordingBlockActionsHandler>();

        await using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
        await using var scope = provider.CreateAsyncScope();

        var pipeline = new ApplicationBuilder(provider).UseSlackbot(enableAuth: false).Build();

        var ctx = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
        ctx.Request.Method = "POST";
        ctx.Request.ContentType = "application/x-www-form-urlencoded";
        ctx.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("payload=" + WebUtility.UrlEncode(payloadJson)));
        ctx.Response.Body = new MemoryStream();

        await pipeline(ctx);

        return (RecordingBlockActionsHandler)scope.ServiceProvider.GetRequiredService<IHandleInteractiveBlockActions>();
    }

    private sealed class RecordingBlockActionsHandler : IHandleInteractiveBlockActions
    {
        public List<BlockActionInteraction> Received { get; } = [];

        public Task<EventHandledResponse> Handle(BlockActionInteraction blockActionEvent)
        {
            Received.Add(blockActionEvent);
            return Task.FromResult(new EventHandledResponse("OK"));
        }
    }
}
