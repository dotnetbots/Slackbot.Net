using System.Net;
using System.Text;

namespace Slackbot.Net.Tests.Helpers;

// Returns a canned JSON body for any request, so the real SlackClient deserialization
// path (including its JsonSerializerOptions/naming policy) runs without hitting Slack.
public class StubHttpMessageHandler(string json) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });
    }
}
