using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Connections;

namespace Slackbot.Net
{
    internal class SlackRtmConnectionHostedService : CronBackgroundService
    {
        public SlackRtmConnectionHostedService(SlackConnectionSetup setup, ILoggerFactory loggerFactory)
        : base(new WorkspaceConnectorRecurringAction(setup), loggerFactory.CreateLogger<WorkspaceConnectorRecurringAction>())
        {
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            // execute on startup in addition to every X as by CRON expr.
            // await Action.Process();
            await base.StartAsync(cancellationToken);
        }
    }

    internal class WorkspaceConnectorRecurringAction : IRecurringAction
    {
        private readonly SlackConnectionSetup _setup;

        public WorkspaceConnectorRecurringAction(SlackConnectionSetup setup)
        {
            _setup = setup;
        }

        public async Task Process()
        {
            await _setup.TryConnectWorkspaces();
        }

        public string Cron { get; } = "*/10 * * * * *";
    }
}