using Microsoft.Extensions.Logging;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

// Self-contained: deserializes realistic Slack payloads through the real SlackClient
// path (no network, no credentials), asserting the bot-author fields populate.
public class ConversationsBotMessageDeserializationTests(ITestOutputHelper helper)
{
    private ISlackClient ClientReturning(string json) =>
        new SlackClient(
            new HttpClient(new StubHttpMessageHandler(json)) { BaseAddress = new Uri("https://slack.com/api/") },
            new XUnitLogger<ISlackClient>(helper));

    // Based on https://docs.slack.dev/reference/events/message/bot_message
    private const string BotMessageJson = """
        {
            "type": "message",
            "subtype": "bot_message",
            "text": "Pushing is the answer",
            "ts": "1358877455.000010",
            "username": "github",
            "bot_id": "BB12033",
            "bot_profile": { "id": "BB12033", "app_id": "A123ABC456", "name": "github" }
        }
        """;

    private const string HumanMessageJson = """
        { "type": "message", "user": "U123ABC456", "text": "hello there", "ts": "1512085950.000216" }
        """;

    [Fact]
    public async Task ConversationsReplies_PopulatesBotFields_OnBotMessage()
    {
        var json = $$"""
            { "ok": true, "messages": [ {{HumanMessageJson}}, {{BotMessageJson}} ], "has_more": false }
            """;
        var response = await ClientReturning(json).ConversationsReplies("C0EC3DG5N", "1358877455.000010");

        Assert.True(response.Ok);

        var bot = Assert.Single(response.Messages, m => m.Bot_Id != null);
        Assert.Equal("BB12033", bot.Bot_Id);
        Assert.Equal("bot_message", bot.SubType);
        Assert.NotNull(bot.Bot_Profile);
        Assert.Equal("github", bot.Bot_Profile.Name);
        Assert.Equal("A123ABC456", bot.Bot_Profile.App_Id);

        var human = Assert.Single(response.Messages, m => m.User == "U123ABC456");
        Assert.Null(human.Bot_Id);
        Assert.Null(human.SubType);
        Assert.Null(human.Bot_Profile);
    }

    [Fact]
    public async Task ConversationsHistory_PopulatesBotFields_AndPagination()
    {
        var json = $$"""
            {
                "ok": true,
                "messages": [ {{BotMessageJson}}, {{HumanMessageJson}} ],
                "has_more": true,
                "response_metadata": { "next_cursor": "bmV4dF90czoxNTEyMDg1ODYxMDAwNTQz" }
            }
            """;
        var response = await ClientReturning(json).ConversationsHistory("C0EC3DG5N");

        Assert.True(response.Ok);
        Assert.True(response.Has_More);
        Assert.Equal("bmV4dF90czoxNTEyMDg1ODYxMDAwNTQz", response.Response_Metadata.Next_Cursor);

        var bot = Assert.Single(response.Messages, m => m.Bot_Id != null);
        Assert.Equal("BB12033", bot.Bot_Id);
        Assert.Equal("bot_message", bot.SubType);
        Assert.Equal("github", bot.Bot_Profile.Name);
    }
}
