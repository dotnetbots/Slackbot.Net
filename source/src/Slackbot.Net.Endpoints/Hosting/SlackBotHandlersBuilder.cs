using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting;

public class SlackBotHandlersBuilder(IServiceCollection services) : ISlackbotHandlersBuilder
{
    public ISlackbotHandlersBuilder AddAppMentionHandler<T>() where T : class, IHandleAppMentions
    {
        services.AddScoped<IHandleAppMentions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddMemberJoinedChannelHandler<T>() where T : class, IHandleMemberJoinedChannel
    {
        services.AddScoped<IHandleMemberJoinedChannel, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddViewSubmissionHandler<T>() where T : class, IHandleViewSubmissions
    {
        services.AddScoped<IHandleViewSubmissions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddInteractiveBlockActionsHandler<T>()
        where T : class, IHandleInteractiveBlockActions
    {
        services.AddScoped<IHandleInteractiveBlockActions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddAppHomeOpenedHandler<T>() where T : class, IHandleAppHomeOpened
    {
        services.AddScoped<IHandleAppHomeOpened, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddShortcut<T>() where T : class, IShortcutAppMentions
    {
        services.AddScoped<IShortcutAppMentions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddNoOpAppMentionHandler<T>() where T : class, INoOpAppMentions
    {
        services.AddScoped<INoOpAppMentions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddMessageActionsHandler<T>() where T : class, IHandleMessageActions
    {
        services.AddScoped<IHandleMessageActions, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddTeamJoinHandler<T>() where T : class, IHandleTeamJoin
    {
        services.AddScoped<IHandleTeamJoin, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddEmojiChangedHandler<T>() where T : class, IHandleEmojiChanged
    {
        services.AddScoped<IHandleEmojiChanged, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddMessageHandler<T>() where T : class, IHandleMessage
    {
        services.AddScoped<IHandleMessage, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddReactionAddedHandler<T>() where T : class, IHandleReactionAdded
    {
        services.AddScoped<IHandleReactionAdded, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddAssistantThreadStartedHandler<T>()
        where T : class, IHandleAssistantThreadStarted
    {
        services.AddScoped<IHandleAssistantThreadStarted, T>();
        return this;
    }

    public ISlackbotHandlersBuilder AddAssistantThreadContextChangedHandler<T>()
        where T : class, IHandleAssistantThreadContextChanged
    {
        services.AddScoped<IHandleAssistantThreadContextChanged, T>();
        return this;
    }
}
