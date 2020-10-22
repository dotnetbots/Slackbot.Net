using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting
{
    public interface ISlackbotHandlersBuilder
    {
        public ISlackbotHandlersBuilder AddHandler<T>() where T:class,IHandleAppMentionEvent;
        public ISlackbotHandlersBuilder AddShortcut<T>() where T:class,IShortcutHandler;
        ISlackbotHandlersBuilder AddViewSubmissionHandler<T>() where T : class, IHandleViewSubmissions;
    }
}