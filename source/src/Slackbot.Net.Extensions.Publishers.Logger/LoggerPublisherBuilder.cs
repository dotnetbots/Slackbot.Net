using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Extensions.Publishers.Logger
{
    public class LoggerPublisherBuilder : IPublisherBuilder
    {
        private readonly ILogger<LoggerPublisher> _logger;

        public LoggerPublisherBuilder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<LoggerPublisher>();
        }
        
        public Task<IPublisher> Build(string slackTeamId)
        {
            return Task.FromResult((IPublisher) new LoggerPublisher(_logger));
        }
    }
}