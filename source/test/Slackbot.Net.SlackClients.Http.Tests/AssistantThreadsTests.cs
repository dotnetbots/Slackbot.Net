using System.Text.Json;
using System.Text.Json.Serialization;
using Slackbot.Net.SlackClients.Http.Models.Requests.AssistantThreadsSetStatus;
using Slackbot.Net.SlackClients.Http.Models.Requests.AssistantThreadsSetSuggestedPrompts;
using Slackbot.Net.SlackClients.Http.Models.Requests.AssistantThreadsSetTitle;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

/// <summary>
/// Verifies the assistant.threads.* request models serialize to the snake_case wire format Slack expects.
/// Mirrors the serialization configured in HttpClientExtensions (Web defaults + WhenWritingNull + a
/// lowercasing naming policy), so it catches the pitfall where e.g. a `ThreadTs` property would
/// wrongly serialize to `threadts` instead of `thread_ts`.
/// </summary>
public class AssistantThreadsSerializationTests
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = new LowerCaseNamingPolicy()
    };

    private class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }

    [Fact]
    public void SetStatusSerializesToSnakeCase()
    {
        var json = JsonSerializer.Serialize(
            new AssistantThreadsSetStatusRequest { Channel_Id = "D1", Thread_Ts = "1.2", Status = "is thinking..." },
            Options);

        Assert.Contains("\"channel_id\":\"D1\"", json);
        Assert.Contains("\"thread_ts\":\"1.2\"", json);
        Assert.Contains("\"status\":\"is thinking...\"", json);
    }

    [Fact]
    public void SetTitleSerializesToSnakeCase()
    {
        var json = JsonSerializer.Serialize(
            new AssistantThreadsSetTitleRequest { Channel_Id = "D1", Thread_Ts = "1.2", Title = "A title" },
            Options);

        Assert.Contains("\"channel_id\":\"D1\"", json);
        Assert.Contains("\"thread_ts\":\"1.2\"", json);
        Assert.Contains("\"title\":\"A title\"", json);
    }

    [Fact]
    public void SetSuggestedPromptsSerializesPromptObjects()
    {
        var json = JsonSerializer.Serialize(
            new AssistantThreadsSetSuggestedPromptsRequest
            {
                Channel_Id = "D1",
                Thread_Ts = "1.2",
                Prompts = new[] { new AssistantThreadPrompt { Title = "Ideas", Message = "Give me ideas" } }
            },
            Options);

        Assert.Contains("\"channel_id\":\"D1\"", json);
        Assert.Contains("\"thread_ts\":\"1.2\"", json);
        Assert.Contains("\"prompts\":[", json);
        Assert.Contains("\"title\":\"Ideas\"", json);
        Assert.Contains("\"message\":\"Give me ideas\"", json);
    }

    [Fact]
    public void SetSuggestedPromptsOmitsNullTitle()
    {
        var json = JsonSerializer.Serialize(
            new AssistantThreadsSetSuggestedPromptsRequest
            {
                Channel_Id = "D1",
                Thread_Ts = "1.2",
                Prompts = new[] { new AssistantThreadPrompt { Title = "Ideas", Message = "Give me ideas" } }
            },
            Options);

        Assert.DoesNotContain("\"title\":null", json);
    }
}

/// <summary>
/// Live integration tests, consistent with the rest of this project. They require the bot token env var
/// (see <see cref="Setup"/>) AND a real assistant thread in an assistant-enabled workspace, so the
/// Channel_Id/Thread_Ts below must be replaced with real values before running manually.
/// </summary>
public class AssistantThreadsTests(ITestOutputHelper helper) : Setup(helper)
{
    private const string AssistantChannelId = "D000000000";
    private const string AssistantThreadTs = "0000000000.000000";

    [Fact(Skip = "Requires a real assistant thread; set AssistantChannelId/AssistantThreadTs and run manually.")]
    public async Task SetStatusWorks()
    {
        var response = await SlackClient.AssistantThreadsSetStatus(new AssistantThreadsSetStatusRequest
        {
            Channel_Id = AssistantChannelId, Thread_Ts = AssistantThreadTs, Status = "is thinking..."
        });
        Assert.True(response.Ok);
    }

    [Fact(Skip = "Requires a real assistant thread; set AssistantChannelId/AssistantThreadTs and run manually.")]
    public async Task SetTitleWorks()
    {
        var response = await SlackClient.AssistantThreadsSetTitle(new AssistantThreadsSetTitleRequest
        {
            Channel_Id = AssistantChannelId, Thread_Ts = AssistantThreadTs, Title = "Test title"
        });
        Assert.True(response.Ok);
    }

    [Fact(Skip = "Requires a real assistant thread; set AssistantChannelId/AssistantThreadTs and run manually.")]
    public async Task SetSuggestedPromptsWorks()
    {
        var response = await SlackClient.AssistantThreadsSetSuggestedPrompts(new AssistantThreadsSetSuggestedPromptsRequest
        {
            Channel_Id = AssistantChannelId,
            Thread_Ts = AssistantThreadTs,
            Title = "What can I help with?",
            Prompts = new[]
            {
                new AssistantThreadPrompt { Title = "Generate ideas", Message = "Give me some marketing ideas" },
                new AssistantThreadPrompt { Title = "Explain a concept", Message = "Explain compound interest" }
            }
        });
        Assert.True(response.Ok);
    }
}
