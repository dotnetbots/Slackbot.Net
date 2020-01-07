using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.SlackClients.Http.Configurations;
using Slackbot.Net.SlackClients.Http.Configurations.Options;

namespace Slackbot.Net.SlackClients.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSlackbotClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BotTokenClientOptions>(configuration);
            services.BuildSlackClient();
            return services;
        }
        
        public static IServiceCollection AddSlackbotClient(this IServiceCollection services, Action<BotTokenClientOptions> configAction)
        {
            services.Configure<BotTokenClientOptions>(configAction);
            services.BuildSlackClient();
            return services;
        }
        private static void BuildSlackClient(this IServiceCollection services)
        {
            services.ConfigureOptions<HttpClientConfigurator>();
            services.AddHttpClient(nameof(SlackClient)).AddTypedClient<ISlackClient, SlackClient>();
        }

        public static IServiceCollection AddSlackbotOauthClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OauthTokenClientOptions>(configuration);
            services.BuildOauthClient();
            return services;
        }

        public static IServiceCollection AddSlackbotOauthClient(this IServiceCollection services, Action<OauthTokenClientOptions> configAction)
        {
            services.Configure<OauthTokenClientOptions>(configAction);
            services.BuildOauthClient();
            return services;
        }

        private static void BuildOauthClient(this IServiceCollection services)
        {
            services.ConfigureOptions<HttpClientConfigurator>();
            services.AddHttpClient(nameof(SearchClient)).AddTypedClient<ISearchClient, SearchClient>();
        }
    }
}