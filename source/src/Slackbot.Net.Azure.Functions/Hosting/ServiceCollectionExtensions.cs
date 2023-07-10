using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Azure.Functions.Abstractions;
using Slackbot.Net.Azure.Functions.Configurations;

namespace Slackbot.Net.Azure.Functions.Hosting;

public static class ServiceCollectionExtensions
{
    public static ISlackbotHandlersBuilder AddSlackBotEvents(this IServiceCollection services)
    {
        services.AddSingleton<ISelectAppMentionEventHandlers, AppMentionEventHandlerSelector>();
        return new SlackBotHandlersBuilder(services);
    }
        
    public static ISlackbotHandlersBuilder AddSlackBotEvents<T>(this IServiceCollection services) where T: class, ITokenStore
    {
        services.AddSingleton<ITokenStore, T>();
        return services.AddSlackBotEvents();
    }
}

public class OAuthOptions
{
    public OAuthOptions()
    {
        OnSuccess = (_,_,_) => Task.CompletedTask;
    }
    public string CLIENT_ID { get; set; }
    public string CLIENT_SECRET { get; set; }
    public string SuccessRedirectUri { get; set; } = "/success?default=1";
    public Func<string, string, IServiceProvider, Task> OnSuccess { get; set; }
}
