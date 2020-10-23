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

```csharp

services.AddSlackBotEvents<MyTokenStore>()
        .AddAppMentionHandler<PublicJokeHandler>();
```

and 

```csharp
 app.UseSlackbot("/events");
 ```

 ### Samples

 Check the [samples](/source/Samples/).