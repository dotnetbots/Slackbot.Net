using System;
using Microsoft.Extensions.Configuration;
using Slackbot.Net;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Configuration;
using Slackbot.Net.Connections;
using Slackbot.Net.Handlers;
using Slackbot.Net.Hosting;
using Slackbot.Net.SlackClients.Http.Extensions;
using Slackbot.Net.Validations;

// namespace on purpose:
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SlackbotWorkerBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddSlackbotWorker(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAndValidate<SlackOptions>(configuration);
            var builder = new SlackbotWorkerBuilder(services);
            builder.AddWorkerDependencies(configuration);
            return builder;
        }

        public static ISlackbotWorkerBuilder AddSlackbotWorker(this IServiceCollection services, Action<SlackOptions> action)
        {
            services.ConfigureAndValidate(action);
            var builder = new SlackbotWorkerBuilder(services);
            builder.AddWorkerDependencies(action);
            return builder;
        }

        private static void AddWorkerDependencies(this ISlackbotWorkerBuilder builder, IConfiguration configuration)
        {
            var slackOptions = new SlackOptions();
            configuration.Bind(slackOptions);
            builder.Services.AddSlackbotClient(o => o.BotToken = slackOptions.Slackbot_SlackApiKey_BotUser);
            builder.Services.AddSlackbotOauthClient(o => o.OauthToken = slackOptions.Slackbot_SlackApiKey_SlackApp);
            AddWorker(builder);
        }
        
        private static void AddWorkerDependencies(this ISlackbotWorkerBuilder builder, Action<SlackOptions> configuration)
        {
            var slackOptions = new SlackOptions();
            configuration(slackOptions);
            builder.Services.AddSlackbotClient(c =>
            {
                c.BotToken = slackOptions.Slackbot_SlackApiKey_BotUser;
            });
            builder.Services.AddSlackbotOauthClient(c =>
            {
                c.OauthToken = slackOptions.Slackbot_SlackApiKey_SlackApp;
            });
            AddWorker(builder);
        }

        private static void AddWorker(ISlackbotWorkerBuilder builder)
        {
            builder.Services.AddSingleton<SlackConnectionSetup>();
            builder.Services.AddSingleton(s => s.GetService<SlackConnectionSetup>().GetBotDetails());
 
            builder.Services.AddSingleton<HandlerSelector>();
            builder.Services.AddHostedService<SlackRtmConnectionHostedService>();
        }
    }
}