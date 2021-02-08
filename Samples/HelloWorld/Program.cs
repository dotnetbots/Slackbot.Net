using HelloWorld.EventHandlers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.SlackClients.Http.Extensions;

namespace HelloWorld
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices(services =>
                    {
                        services.AddSlackClientBuilder();
                        services.AddSlackBotEvents<MyTokenStore>()
                            .AddAppMentionHandler<PublicJokeHandler>()
                            .AddAppMentionHandler<HiddenTestHandler>()
                            .AddAppMentionHandler<HelloWorldHandler>()
                            .AddMemberJoinedChannelHandler<MemberJoinedChannelHandler>()
                            .AddShortcut<ListPublicCommands>()
                            .AddViewSubmissionHandler<AppHomeViewSubmissionHandler>()
                            .AddAppHomeOpenedHandler<AppHomeOpenedHandler>();
                    });
                    webBuilder.Configure(app => app.UseSlackbot());
                });
    }
}