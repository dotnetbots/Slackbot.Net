using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;

namespace Slackbot.Net.Azure.Functions.Triggers;

public static class SlackbotHttpTrigger
{
    [Function("SlackbotHttpFunction")]
    public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "slack/event")] HttpRequest req)
    {
    }
}
