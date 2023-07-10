using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Azure.Functions.Abstractions;
using Slackbot.Net.Azure.Functions.Models.Events;
using Slackbot.Net.SlackClients.Http;

public class AzureFunctionAppMentionHandler : IHandleAppMentions
{
    private readonly ISlackClient _client;
    private readonly ILogger<AzureFunctionAppMentionHandler> _logger;

    public AzureFunctionAppMentionHandler(ISlackClient client, ILogger<AzureFunctionAppMentionHandler> logger)
    {
        _client = client;
        _logger = logger;
    }
    public async Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        _logger.LogInformation("Hello, Azure Functions!");

        // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CS4014
        _client.ChatPostMessage(slackEvent.Channel, $"Pling Pong, from Azure Functions!");
#pragma warning restore CS4014

        return new EventHandledResponse("OK");
    }
}
