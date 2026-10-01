using System.Net;

namespace Slackbot.Net.SlackClients.Http.Exceptions;

public class SlackRateLimitedException : HttpRequestException
{
    public SlackRateLimitedException(TimeSpan retryAfter, string responseContent)
        : base($"Slack rate limited the request. Retry after {retryAfter.TotalSeconds} seconds.", null, HttpStatusCode.TooManyRequests)
    {
        RetryAfter = retryAfter;
        ResponseContent = responseContent;
    }

    public TimeSpan RetryAfter { get; }

    public string ResponseContent { get; }
}
