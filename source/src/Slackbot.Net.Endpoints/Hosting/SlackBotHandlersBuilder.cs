using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting;

public class SlackBotHandlersBuilder(IServiceCollection services) : ISlackbotHandlersBuilder
{
    public ISlackbotHandlersBuilder AddAppMentionHandler<T>() where T : class, IHandleAppMentions
    {
        services.AddSingleton<IHandleAppMentions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddMemberJoinedChannelHandler<T>() where T : class, IHandleMemberJoinedChannel
    {
        services.AddSingleton<IHandleMemberJoinedChannel, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddViewSubmissionHandler<T>() where T : class, IHandleViewSubmissions
    {
        services.AddSingleton<IHandleViewSubmissions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddInteractiveBlockActionsHandler<T>()
        where T : class, IHandleInteractiveBlockActions
    {
        services.AddSingleton<IHandleInteractiveBlockActions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddAppHomeOpenedHandler<T>() where T : class, IHandleAppHomeOpened
    {
        services.AddSingleton<IHandleAppHomeOpened, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddShortcut<T>() where T : class, IShortcutAppMentions
    {
        services.AddSingleton<IShortcutAppMentions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddNoOpAppMentionHandler<T>() where T : class, INoOpAppMentions
    {
        services.AddSingleton<INoOpAppMentions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddMessageActionsHandler<T>() where T : class, IHandleMessageActions
    {
        services.AddSingleton<IHandleMessageActions, T>();
        return this;
    }
}
