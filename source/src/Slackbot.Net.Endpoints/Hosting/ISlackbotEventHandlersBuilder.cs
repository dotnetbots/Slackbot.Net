using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting
{
    public interface ISlackbotEventHandlersBuilder
    {
        public ISlackbotEventHandlersBuilder AddHandler<T>() where T:class,IHandleEvent;
        public ISlackbotEventHandlersBuilder AddShortcut<T>() where T:class,IShortcutHandler;
    }
}