# Slackbot.NET


[![Build](https://github.com/slackbot-net/slackbot.net/workflows/CI/badge.svg)](https://github.com/slackbot.net/slackbot.net/actions)


## What?
An opinionated ASP.NET Core middleware to handle Slack Event payloads.

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
                .AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("secret"));

// Setup event handlers
builder.Services.AddSlackBotEvents<MyTokenStore>()
                .AddAppMentionHandler<DoStuff>()


var app = builder.Build();
app.UseSlackbot(); // event endpoint
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

class MyTokenStore : ITokenStore
{
    public string SlackToken = Environment.GetEnvironmentVariable("SLACK_TOKEN");

    public Task<IEnumerable<string>> GetTokens() => Task.FromResult(new[] { SlackToken });
    public Task<string> GetTokenByTeamId(string teamId) => Task.FromResult(SlackToken);
    public Task Delete(string token) => throw new NotImplementedException("Single workspace app");
    public Task Insert(Workspace slackTeam) => throw new NotImplementedException("Single workspace app");
}
 ```

 ### Advanced: Distributed Slack app

 Implement the `ITokenStore` and store/retrieve tokens to any storage.

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
    public Task<IEnumerable<string>> GetTokens() => _db.AllTokens();
    public Task<string> GetTokenByTeamId(string teamId) => _db.FetchById(teamId);
    public Task Delete(string token) => _db.DeleteByToken(token);
    public Task Insert(Workspace slackTeam) => db.Insert(slackTeam);
}
 ```

 ### Samples

 Check the [samples](/Samples/).
