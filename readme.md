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



### Configuration


A complete ASP.NET Core 6 Slackbot:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Needed to verify that incoming event payloads are from Slack
builder.Services.AddAuthentication()
                .AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("secret"));

// Setup event handlers
builder.Services.AddSlackBotEvents<MyTokenStore>()
                .AddAppMentionHandler<DoStuff>()


var app = builder.Build();
app.UseSlackbot(); //or app.UseSlackbot(enableAuth:false) during development
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
}
 ```

A slack app can be distributed either as a single-workspace application (1 single Slack token), or as a _distributed_ Slack application where other workspaces can install it them self either via a web page, or via the Slack App Store.
 ### Single workspace Slack app

 Implement the `ITokenStore`, and return a single workspace token from relevant methods (get/get-all)


 ### Advanced: Distributed Slack app

 Implement the `ITokenStore` and store/retrieve tokens to any storage. The install and uninstall flows are currently not part of this library, so you would have to implement the relevant redirects triggers and callbacks endpoints yourself. Those callback endpoints needs to save the tokens to the storage used in your `ITokenStore` implementation.

 A typical callback endpoint would consist of:
 1) Receiving the OAuth2 code as query string as part of (the final redirect of the OAuth2 authorization code flow)
 2) On receive, a server-to-server API request to exchange the code for an Slack token for the installing workspace. Via your apps ClientId, ClientSecret and the OAuth2 code, use the Slack OAuthAccessV2 HTTP API here.
 3) On a successful exchange, save the WorkspaceId and the token to your storage.

 Providing an OOB solution for distributed apps is on the roadmap.

 ### Samples

 Check the [samples](/Samples/).
