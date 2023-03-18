using System.Text.Json;
using System.Text.Json.Serialization;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.Authentication;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;
using Slackbot.Net.Endpoints.Models.Interactive.MessageActions;

var builder = WebApplication.CreateBuilder(args);

// Needed in production to verify that incoming event payloads are from Slack
builder.Services.AddAuthentication()
                .AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("CLIENT_SIGNING_SECRET"));

// Setup event handlers
builder.Services.AddSlackBotEvents()
    .AddMessageActionsHandler<DoOtherStuff>()
    .AddAppMentionHandler<DoStuff>();


var app = builder.Build();
app.UseSlackbot(enableAuth:!app.Environment.IsDevelopment()); // disable during development for easier testing
app.Run();

class DoStuff : IHandleAppMentions
{
    public bool ShouldHandle(AppMentionEvent slackEvent) => slackEvent.Text.Contains("dostuff");

    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        Console.WriteLine("Doing stuff!");
        return Task.FromResult(new EventHandledResponse("yolo"));
    }
}

class DoOtherStuff : IHandleMessageActions
{
    public async Task<EventHandledResponse> Handle(MessageActionInteraction @event)
    {
        var str = JsonSerializer.Serialize(@event);
        var httpClient = new HttpClient();
        var res = await httpClient.PostAsJsonAsync(@event.Response_Url, new
        {
            text = "Thx"
        });
        Console.WriteLine(str);
        return new EventHandledResponse("OK");
    }
}
