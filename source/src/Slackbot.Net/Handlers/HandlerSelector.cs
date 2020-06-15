using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Handlers.Models.Rtm.MessageReceived;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.Dynamic;
using Slackbot.Net.SlackClients.Http;

namespace Slackbot.Net.Handlers
{
    public class HandlerSelector
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
            try
            {
                allHandlers = _provider.GetServices<IHandleMessages>();
            }
            catch (InvalidOperationException ioe)
            {
                if (ioe.Message.Contains("Unable to resolve") && ioe.Message.Contains("IPublisherBuilder"))
                {
                    _logger.LogError("Attempted to use IPublisherBuilderFactory in a IHandleMessages implementation, but none were registered.\n" +
                                     "Install one via NuGet, and add it using `AddSlackbotWorker().AddXXPublisher`\n" +
                                     "Examples: AddLoggerPublisher, AddSlackPublisher");
                }
                if (ioe.Message.Contains("Unable to resolve") && ioe.Message.Contains("IPublisher"))
                {
                    _logger.LogError("Attempted to use IPublisher in a IHandleMessages implementation, but this type is deprecated.\n" +
                                     "Use IPublisherBuilderFactory instead.");
                }
                else
                {
                    _logger.LogError(ioe.Message, ioe);
                }

                throw ioe;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw e;
            }
            
            if (allHandlers == null)
                throw new Exception("No handlers registred");

            allHandlers = allHandlers.ToList();

            var service = _provider.GetService<ISlackClientService>();
            var slackClient = await service.CreateClient(message.Team.Id);
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

    internal class SlackbotNetSetupException : Exception
    {
        public SlackbotNetSetupException(string msg, Exception ioe) : base(msg, ioe)
        {
        }
    }
}