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

        // Backwards compatibility: if T still implements the obsolete ITokenStore interface,
        // make it resolvable that way too, so existing code depending on ITokenStore keeps working.
#pragma warning disable CS0618 // Type or member is obsolete
        if (typeof(ITokenStore).IsAssignableFrom(typeof(T)))
        {
            services.AddScoped<ITokenStore>(sp => (ITokenStore)sp.GetRequiredService<IWorkspaceInstallationHandler>());
        }
#pragma warning restore CS0618

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
    public string SuccessRedirectUri { get; set; } = "/success?default=1";

    [Obsolete("Put post-install logic directly in your IWorkspaceInstallationHandler.Install implementation instead. OnSuccess will be removed in a future version.")]
    public Func<string, string, IServiceProvider, Task> OnSuccess { get; set; } = (_, _, _) => Task.CompletedTask;
}
