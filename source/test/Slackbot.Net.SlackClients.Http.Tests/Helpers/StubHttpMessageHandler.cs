using System.Net;
using System.Text;

namespace Slackbot.Net.Tests.Helpers;

// Returns a canned JSON body for any request, so the real SlackClient deserialization
// path (including its JsonSerializerOptions/naming policy) runs without hitting Slack.
public class StubHttpMessageHandler(string json, HttpStatusCode statusCode = HttpStatusCode.OK, Action<HttpResponseMessage> configureResponse = null) : HttpMessageHandler
{
    public string LastRequestContent { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        LastRequestContent = request.Content is null ? null : await request.Content.ReadAsStringAsync(cancellationToken);
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        configureResponse?.Invoke(response);
        return response;
    }
}
