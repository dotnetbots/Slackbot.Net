using Microsoft.Extensions.Logging;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

// Self-contained: deserializes realistic Slack payloads through the real SlackClient
// path (no network, no credentials).
public class ConversationsInfoAndMembersDeserializationTests(ITestOutputHelper helper)
{
    private ISlackClient ClientReturning(string json) =>
        new SlackClient(
            new HttpClient(new StubHttpMessageHandler(json)) { BaseAddress = new Uri("https://slack.com/api/") },
            new XUnitLogger<ISlackClient>(helper));

    // Based on https://docs.slack.dev/reference/methods/conversations.info
    [Fact]
    public async Task ConversationsInfo_PopulatesNumMembers_WhenRequested()
    {
        var json = """
            {
                "ok": true,
                "channel": {
                    "id": "C012AB3CD",
                    "name": "general",
                    "is_channel": true,
                    "is_archived": false,
                    "num_members": 6
                }
            }
            """;
        var response = await ClientReturning(json).ConversationsInfo("C012AB3CD", includeNumMembers: true);

        Assert.True(response.Ok);
        Assert.Equal("C012AB3CD", response.Channel.Id);
        Assert.Equal("general", response.Channel.Name);
        Assert.True(response.Channel.Is_Channel);
        Assert.False(response.Channel.Is_Archived);
        Assert.Equal(6, response.Channel.Num_Members);
    }

    // num_members is only present when include_num_members=true was passed on the request.
    [Fact]
    public async Task ConversationsInfo_NumMembersIsNull_WhenNotRequested()
    {
        var json = """
            { "ok": true, "channel": { "id": "C012AB3CD", "name": "general", "is_channel": true, "is_archived": false } }
            """;
        var response = await ClientReturning(json).ConversationsInfo("C012AB3CD");

        Assert.True(response.Ok);
        Assert.Null(response.Channel.Num_Members);
    }

    // Based on https://docs.slack.dev/reference/methods/conversations.members - a flat list of
    // member user ids, not a list of Conversation objects.
    [Fact]
    public async Task ConversationsMembers_PopulatesMemberIds_AndPagination()
    {
        var json = """
            {
                "ok": true,
                "members": ["U023BECGF", "U061F7AUR", "W012A3CDE"],
                "response_metadata": { "next_cursor": "dXNlcjpVMDYxTkZUVDI=" }
            }
            """;
        var response = await ClientReturning(json).ConversationsMembers("C012AB3CD");

        Assert.True(response.Ok);
        Assert.Equal(["U023BECGF", "U061F7AUR", "W012A3CDE"], response.Members);
        Assert.Equal("dXNlcjpVMDYxTkZUVDI=", response.Response_Metadata.Next_Cursor);
    }
}
