using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace Slackbot.Net.Endpoints
{
    internal class EventHandlerSelector : ISelectEventHandlers
    {
        private readonly ILogger<EventHandlerSelector> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IServiceProvider _provider;

        public EventHandlerSelector(ILoggerFactory loggerFactory, IServiceProvider provider)
        {
            _logger = loggerFactory.CreateLogger<EventHandlerSelector>();
            _loggerFactory = loggerFactory;
            _provider = provider;
        }
        
        public async Task<IEnumerable<IHandleAppMentionEvent>> GetAppMentionEventHandlerFor(EventMetaData eventMetadata, AppMentionEvent slackEvent)
        {
            var allHandlers = _provider.GetServices<IHandleAppMentionEvent>();
            var shortCutter = _provider.GetService<IShortcutHandler>();
            
            if(shortCutter == null)
                shortCutter = new NoOpShortcuttingHandler();

            if (shortCutter.ShouldHandle(slackEvent))
            {
                await shortCutter.Handle(eventMetadata, slackEvent);
                return new List<IHandleAppMentionEvent>();
            }

            return SelectHandler(allHandlers, slackEvent);
 
        }

        private IEnumerable<IHandleAppMentionEvent> SelectHandler(IEnumerable<IHandleAppMentionEvent> handlers, AppMentionEvent message)
        {
            var matchingHandlers = handlers.Where(s => s.ShouldHandle(message));
            if (matchingHandlers.Any())
                return matchingHandlers;

            return new List<IHandleAppMentionEvent> {new NoOpAppMentionEventHandler(_loggerFactory.CreateLogger<NoOpAppMentionEventHandler>())};
        }
    }
}