using System.Net;
using System.Net.Http.Headers;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.SlackClients.Http.Exceptions;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class UsersListPagingTests(ITestOutputHelper helper)
{
    private const string PageJson = """
        {
            "ok": true,
            "members": [{ "id": "U023BECGF", "name": "bobby" }],
            "response_metadata": { "next_cursor": "dXNlcjpVMEc5V0ZYTlo=" }
        }
        """;

    private ISlackClient ClientFor(StubHttpMessageHandler handler) =>
        new SlackClient(
            new HttpClient(handler) { BaseAddress = new Uri("https://slack.com/api/") },
            new XUnitLogger<ISlackClient>(helper));

    [Fact]
    public async Task UsersList_SendsLimitAndCursor()
    {
        var handler = new StubHttpMessageHandler(PageJson);

        await ClientFor(handler).UsersList("dXNlcjpVMEc5V0ZYTlo=", 100);

        Assert.Equal("limit=100&cursor=dXNlcjpVMEc5V0ZYTlo%3D", handler.LastRequestContent);
    }

    [Fact]
    public async Task UsersList_OmitsCursor_WhenEmpty()
    {
        var handler = new StubHttpMessageHandler(PageJson);

        await ClientFor(handler).UsersList(null);

        Assert.Equal("limit=200", handler.LastRequestContent);
    }

    [Fact]
    public async Task UsersList_PopulatesNextCursor()
    {
        var response = await ClientFor(new StubHttpMessageHandler(PageJson)).UsersList(null);

        Assert.Equal("U023BECGF", Assert.Single(response.Members).Id);
        Assert.Equal("dXNlcjpVMEc5V0ZYTlo=", response.Response_Metadata.Next_Cursor);
    }

    [Fact]
    public async Task UsersList_ThrowsRateLimited_WithRetryAfter()
    {
        var handler = new StubHttpMessageHandler("""{ "ok": false, "error": "ratelimited" }""", HttpStatusCode.TooManyRequests,
            r => r.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(30)));

        var exception = await Assert.ThrowsAsync<SlackRateLimitedException>(() => ClientFor(handler).UsersList(null));

        Assert.Equal(TimeSpan.FromSeconds(30), exception.RetryAfter);
        Assert.Equal(HttpStatusCode.TooManyRequests, exception.StatusCode);
    }

    [Fact]
    public async Task RateLimited_FallsBackToSixtySeconds_WithoutRetryAfter()
    {
        var handler = new StubHttpMessageHandler("""{ "ok": false, "error": "ratelimited" }""", HttpStatusCode.TooManyRequests);

        var exception = await Assert.ThrowsAsync<SlackRateLimitedException>(() => ClientFor(handler).UsersList());

        Assert.Equal(TimeSpan.FromSeconds(60), exception.RetryAfter);
    }

    [Fact]
    public async Task RateLimited_IsCatchableAsHttpRequestException_OnJsonPosts()
    {
        var handler = new StubHttpMessageHandler("""{ "ok": false, "error": "ratelimited" }""", HttpStatusCode.TooManyRequests,
            r => r.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(5)));

        var exception = await Assert.ThrowsAnyAsync<HttpRequestException>(() => ClientFor(handler).ChatPostMessage(new ChatPostMessageRequest { Channel = "C123", Text = "hi" }));

        Assert.Equal(TimeSpan.FromSeconds(5), Assert.IsType<SlackRateLimitedException>(exception).RetryAfter);
    }
}
