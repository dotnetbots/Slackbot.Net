using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.SlackClients.Http.Configurations;
using Slackbot.Net.SlackClients.Http.Configurations.Options;

namespace Slackbot.Net.SlackClients.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSlackHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BotTokenClientOptions>(configuration);
            services.BuildSlackClient();
            return services;
        }
        
        public static IServiceCollection AddSlackHttpClient(this IServiceCollection services, Action<BotTokenClientOptions> configAction)
        {
            services.Configure<BotTokenClientOptions>(configAction);
            services.BuildSlackClient();
            return services;
        }

        public static IServiceCollection AddSlackClientBuilder(this IServiceCollection services)
        {
            services.AddSingleton<ISlackClientBuilder, SlackClientBuilder>();
            return services;
        }
        
        private static void BuildSlackClient(this IServiceCollection services)
        {
            services.ConfigureOptions<SlackClientConfigurator>();
            services.AddHttpClient(nameof(SlackClient)).AddTypedClient<ISlackClient, SlackClient>();
        }

        public static IServiceCollection AddSlackOauthHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OauthTokenClientOptions>(configuration);
            services.BuildSearchClient();
            return services;
        }

        public static IServiceCollection AddSlackbotOauthClient(this IServiceCollection services, Action<OauthTokenClientOptions> configAction)
        {
            services.Configure<OauthTokenClientOptions>(configAction);
            services.BuildSearchClient();
            return services;
        }
        
        public static IServiceCollection AddSlackbotOauthAccessHttpClient(this IServiceCollection services)
        {
            services.ConfigureOptions<SlackClientConfigurator>();
            services.AddHttpClient(nameof(SlackOAuthAccessClient)).AddTypedClient<ISlackOAuthAccessClient, SlackOAuthAccessClient>();
            return services;
        }
        
        private static void BuildSearchClient(this IServiceCollection services)
        {
            services.ConfigureOptions<SearchClientConfigurator>();
            services.AddHttpClient(nameof(SearchClient)).AddTypedClient<ISearchClient, SearchClient>();
        }
    }
}