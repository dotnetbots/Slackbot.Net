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
        
        public async Task<IEnumerable<IHandleEvent>> GetEventHandlerFor(EventMetaData eventMetadata, SlackEvent slackEvent)
        {
            var allHandlers = _provider.GetServices<IHandleEvent>();
            var shortCutter = _provider.GetService<IShortcutHandler>();
            
            if(shortCutter == null)
                shortCutter = new NoOpShortcuttingHandler();

            if (shortCutter.ShouldHandle(slackEvent))
            {
                await shortCutter.Handle(eventMetadata, slackEvent);
                return new List<IHandleEvent>();
            }

            return SelectHandler(allHandlers, slackEvent);
 
        }

        private IEnumerable<IHandleEvent> SelectHandler(IEnumerable<IHandleEvent> handlers, SlackEvent message)
        {
            var matchingHandlers = handlers.Where(s => s.ShouldHandle(message));
            if (matchingHandlers.Any())
                return matchingHandlers;

            return new List<IHandleEvent> {new NoOpEventHandler(_loggerFactory.CreateLogger<NoOpEventHandler>())};
        }
    }
}