using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.Authentication;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

var builder = WebApplication.CreateBuilder(args);

// Needed in production to verify that incoming event payloads are from Slack
builder.Services.AddAuthentication()
                .AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("CLIENT_SIGNING_SECRET"));

// Setup event handlers
builder.Services.AddSlackBotEvents()
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