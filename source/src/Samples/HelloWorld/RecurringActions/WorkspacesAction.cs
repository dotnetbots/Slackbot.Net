using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CronBackgroundServices;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Hosting;

namespace HelloWorld.RecurringActions
{
    internal class WorkspacesAction : IRecurringAction
    {
        private readonly ITokenStore _workspaceService;
        private readonly ILogger<WorkspacesAction> _logger;

        public WorkspacesAction(ITokenStore workspaceService, ILogger<WorkspacesAction> logger)
        {
            _workspaceService = workspaceService;
            _logger = logger;
        }

        public async Task Process(CancellationToken stoppingToken)
        {
            var ws = await _workspaceService.GetTokens();
            _logger.LogTrace(string.Join("\n", ws.Select(c => $"Token: {Scrambled(c)}")));
        }

        private string Scrambled(string token)
        {
            if (string.IsNullOrEmpty(token))
                return "<empty> Check config/source?";
            
            if (token.Length > 5)
            {
                var last5 = token.Substring(token.Length - 5);
                var stars = new string('*', token.Length - 5);
                return $"{stars}{last5}";
            }
            
            return new string('*', token.Length);
        }

        public string Cron { get; } = "0 */1 * * * *";
    }
}