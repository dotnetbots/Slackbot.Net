using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.SlackClients.Http;

namespace Slackbot.Net.Handlers
{
    internal class HandlerSelector
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<HandlerSelector> _logger;

        public HandlerSelector(IServiceProvider provider, ILogger<HandlerSelector> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        public async Task HandleIncomingMessage(SlackMessage message)
        {
            IEnumerable<IHandleMessages> allHandlers = null;
            IEnumerable<IPublisher> publishers = null;
            try
            {
                allHandlers = _provider.GetServices<IHandleMessages>();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                throw e;
            }
            
            if (allHandlers == null)
                throw new Exception("No handlers registred");

            allHandlers = allHandlers.ToList();

            var tokenStore = _provider.GetService<ITokenStore>();
            var clientFactory = _provider.GetService<ISlackClientFactory>();
            var tokenForTeam = await tokenStore.GetTokenByTeamId(message.Team.Id);
            var slackClient = clientFactory.Create(tokenForTeam);
            var helpHandler = new HelpHandler(allHandlers, slackClient);

            if (helpHandler.ShouldHandle(message))
            {
                await helpHandler.Handle(message);
                return;
            }

            var handlers = SelectHandler(allHandlers,message);
            foreach (var handler in handlers)
            {
                try
                {
                    await handler.Handle(message);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }

        private IEnumerable<IHandleMessages> SelectHandler(IEnumerable<IHandleMessages> handlers, SlackMessage message)
        {
            var matchingHandlers = handlers.Where(s => s.ShouldHandle(message));
            foreach (var handler in matchingHandlers)
            {
                yield return handler;
            }

            yield return new NoOpHandler();
        }
    }
}