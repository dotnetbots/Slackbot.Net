using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Slackbot.Net.Azure.Functions.Authentication;
using Slackbot.Net.Azure.Functions.Hosting;
using Slackbot.Net.SlackClients.Http.Extensions;

var dotnetEnv = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
var enableAuth = dotnetEnv == "Production";
Console.WriteLine($"DOTNET_ENVIRONMENT:{dotnetEnv}");
var host = new HostBuilder()
    .ConfigureServices(s =>
    {
        s.AddAuthentication()
         .AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("CLIENT_SIGNING_SECRET"));

        s.AddSlackBotEvents().AddAppMentionHandler<AzureFunctionAppMentionHandler>();
        s.AddSlackHttpClient(c => c.BotToken = Environment.GetEnvironmentVariable("SLACK_TOKEN"));
    })
    .ConfigureFunctionsWebApplication(a => a.UseSlackbot(enableAuth:enableAuth))
    .Build();

host.Run();
