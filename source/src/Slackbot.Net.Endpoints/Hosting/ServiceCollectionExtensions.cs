using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.OAuth;

namespace Slackbot.Net.Endpoints.Hosting;

public static class ServiceCollectionExtensions
{
    public static ISlackbotHandlersBuilder AddSlackBotEvents(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<ISelectAppMentionEventHandlers, AppMentionEventHandlerSelector>();
        return new SlackBotHandlersBuilder(services);
    }

    public static ISlackbotHandlersBuilder AddSlackBotEvents<T>(this IServiceCollection services)
        where T : class, IWorkspaceInstallationHandler
    {
        services.AddScoped<IWorkspaceInstallationHandler, T>();
        return services.AddSlackBotEvents();
    }

    public static IServiceCollection AddSlackbotDistribution(this IServiceCollection services,
        Action<OAuthOptions> action)
    {
        services.Configure(action);
        services.AddHttpClient<OAuthClient>((s, c) =>
        {
            c.BaseAddress = new Uri("https://slack.com/api/");
            c.Timeout = TimeSpan.FromSeconds(15);
        });
        return services;
    }
}

public class OAuthOptions
{
    public string CLIENT_ID { get; set; }
    public string CLIENT_SECRET { get; set; }
    /// <summary>
    ///     Where the user ends up after a successful install. If the install was started with an
    ///     OAuth <c>state</c> parameter, that value is appended here as <c>?state=</c> so the page
    ///     can pick it up — this library never interprets it.
    /// </summary>
    public string SuccessRedirectUri { get; set; } = "/success?default=1";
}
