# Slackbot.NET


[![Build](https://github.com/slackbot-net/slackbot.net/workflows/CI/badge.svg)](https://github.com/slackbot.net/slackbot.net/actions)


## What?
An opinionated ASP.NET Core middleware to create simple Slackbots using the Slack event API.

### Install
Download it from NuGet:[![NuGet](https://img.shields.io/nuget/dt/slackbot.net.endpoints.svg)](https://www.nuget.org/packages/slackbot.net.endpoints/)

`$ dotnet add package slackbot.net.endpoints`

### Supported events
- `challenge`
- `app_mention`
- `view_submission`
- `member_joined_channel`



## Configuration

A slack app can be distributed either as a single-workspace application (1 single Slack token), or as a _distributed_ Slack application where other workspaces can install it them self either via a web page, or via the Slack App Store.
 ### Single workspace Slack app


```csharp
var builder = WebApplication.CreateBuilder(args);

// Needed to verify that incoming event payloads are from Slack
builder.Services.AddAuthentication()
                .AddSlackbotEvents(c =>
                    c.SigningSecret = Environment.GetEnvironmentVariable("SIGNING_SECRET")
                );

// Setup event handlers
builder.Services.AddSlackBotEvents()
                .AddAppMentionHandler<DoStuff>()


var app = builder.Build();
app.UseSlackbot(); // event endpoint
app.Run();

class DoStuff : IHandleAppMentions
{
    public bool ShouldHandle(AppMentionEvent slackEvent) => slackEvent.Text.Contains("hello");

    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        Console.WriteLine("Doing stuff!");
        return Task.FromResult(new EventHandledResponse("yolo"));
    }
}
 ```

 ### Advanced: Distributed Slack app

 Implement the `ITokenStore` to store/remote on install/uninstall flows.

 ```csharp
var builder = WebApplication.CreateBuilder(args);

// Needed to verify that incoming event payloads are from Slack
builder.Services.AddAuthentication()
                .AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("SIGNING_SECRET"));

builder.Services.AddSlackbotDistribution(c => {
    c.CLIENT_ID = Environment.GetEnvironmentVariable("CLIENT_ID");
    c.CLIENT_SECRET = Environment.GetEnvironmentVariable("CLIENT_SECRET");
});

// Setup event handlers
builder.Services.AddSlackBotEvents<MyTokenStore>()
                .AddAppMentionHandler<DoStuff>()


var app = builder.Build();
app.Map("/authorize", a => a.UseSlackbotDistribution()); // OAuth callback endpoint
app.Map("/events", a => a.UseSlackbot()); // event endpoint
app.Run();

class DoStuff : IHandleAppMentions
{
    public bool ShouldHandle(AppMentionEvent slackEvent) => slackEvent.Text.Contains("CLIENT_SIGNING_SECRET");

    public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
    {
        Console.WriteLine("Doing stuff!");
        return Task.FromResult(new EventHandledResponse("yolo"));
    }
}

/// Bring-your-own-token-store:
class MyTokenStore : ITokenStore
{
    // _db is some persistance technology you choose (sql, mongo, whatever)
    public Task<Workspace Delete(string teamId) => _db.DeleteByTeamId(teamId);
    public Task Insert(Workspace slackTeam) => _db.Insert(slackTeam);
}
 ```

 ### Samples

 Check the [samples](/Samples/).
