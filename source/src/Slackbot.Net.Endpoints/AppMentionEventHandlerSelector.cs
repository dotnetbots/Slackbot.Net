using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints;

internal class AppMentionEventHandlerSelector(ILoggerFactory loggerFactory, IServiceProvider provider)
    : ISelectAppMentionEventHandlers
{
    public async Task<IEnumerable<IHandleAppMentions>> GetAppMentionEventHandlerFor(EventMetaData eventMetadata,
        AppMentionEvent slackEvent)
    {
        var allHandlers = provider.GetServices<IHandleAppMentions>();
        var shortCutter = provider.GetService<IShortcutAppMentions>();
        var noopHandler = provider.GetService<INoOpAppMentions>();

        if (shortCutter != null && shortCutter.ShouldShortcut(slackEvent))
        {
            await shortCutter.Handle(eventMetadata, slackEvent);
            return new List<IHandleAppMentions>();
        }

        return SelectHandler(allHandlers, noopHandler, slackEvent);
    }

    private IEnumerable<IHandleAppMentions> SelectHandler(IEnumerable<IHandleAppMentions> handlers,
        INoOpAppMentions noOpAppMentions, AppMentionEvent message)
    {
        var matchingHandlers = handlers.Where(s => s.ShouldHandle(message));
        if (matchingHandlers.Any())
        {
            return matchingHandlers;
        }

        if (noOpAppMentions != null)
        {
            return new List<IHandleAppMentions> { noOpAppMentions };
        }

        return new List<IHandleAppMentions>
        {
            new NoOpAppMentionEventHandler(loggerFactory.CreateLogger<NoOpAppMentionEventHandler>())
        };
    }
}
