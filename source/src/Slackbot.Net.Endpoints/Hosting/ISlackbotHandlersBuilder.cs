using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting;

public interface ISlackbotHandlersBuilder
{
    public ISlackbotHandlersBuilder AddAppMentionHandler<T>() where T : class, IHandleAppMentions;
    public ISlackbotHandlersBuilder AddShortcut<T>() where T : class, IShortcutAppMentions;
    public ISlackbotHandlersBuilder AddMemberJoinedChannelHandler<T>() where T : class, IHandleMemberJoinedChannel;
    public ISlackbotHandlersBuilder AddViewSubmissionHandler<T>() where T : class, IHandleViewSubmissions;

    public ISlackbotHandlersBuilder AddInteractiveBlockActionsHandler<T>()
        where T : class, IHandleInteractiveBlockActions;

    public ISlackbotHandlersBuilder AddAppHomeOpenedHandler<T>() where T : class, IHandleAppHomeOpened;
    public ISlackbotHandlersBuilder AddNoOpAppMentionHandler<T>() where T : class, INoOpAppMentions;

    public ISlackbotHandlersBuilder AddMessageActionsHandler<T>() where T : class, IHandleMessageActions;
    public ISlackbotHandlersBuilder AddTeamJoinHandler<T>() where T : class, IHandleTeamJoin;
    public ISlackbotHandlersBuilder AddEmojiChangedHandler<T>() where T : class, IHandleEmojiChanged;
    public ISlackbotHandlersBuilder AddMessageHandler<T>() where T : class, IHandleMessage;
}
