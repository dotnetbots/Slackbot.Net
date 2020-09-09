using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Abstractions;

namespace Slackbot.Net.Endpoints.Hosting
{
    public class SlackBotHandlersBuilder : ISlackbotHandlersBuilder
    {
        private readonly IServiceCollection _services;

        public SlackBotHandlersBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ISlackbotHandlersBuilder AddHandler<T>() where T : class, IHandleEvent
        {
            _services.AddSingleton<IHandleEvent, T>();
            return this;
        }
        
        public ISlackbotHandlersBuilder AddViewSubmissionHandler<T>() where T : class, IHandleViewSubmissions
        {
            _services.AddSingleton<IHandleViewSubmissions, T>();
            return this;
        }

        public ISlackbotHandlersBuilder AddShortcut<T>() where T : class, IShortcutHandler
        {
            _services.AddSingleton<IShortcutHandler, T>();
            return this;
        }
    }
}