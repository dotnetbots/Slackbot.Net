using HelloWorld.EventHandlers;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.SlackClients.Http.Extensions;
using Slackbot.Net.Endpoints.Authentication;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication().AddSlackbotEvents(c => c.SigningSecret = Environment.GetEnvironmentVariable("CLIENT_SIGNING_SECRET"));
builder.Services.AddSlackClientBuilder()
                .AddSlackBotEvents<MyTokenStore>()
                    .AddAppMentionHandler<PublicJokeHandler>()
                    .AddAppMentionHandler<HiddenTestHandler>()
                    .AddAppMentionHandler<HelloWorldHandler>()
                    .AddMemberJoinedChannelHandler<MemberJoinedChannelHandler>()
                    .AddShortcut<ListPublicCommands>()
                    .AddViewSubmissionHandler<AppHomeViewSubmissionHandler>()
                    .AddAppHomeOpenedHandler<AppHomeOpenedHandler>();

var app = builder.Build();
app.UseSlackbot(enableAuth: !app.Environment.IsDevelopment());
app.Run();
