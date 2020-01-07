using System.Threading.Tasks;

namespace Slackbot.Net.Abstractions.Handlers
{
    public interface IRecurringAction
    {
        Task Process();
        string Cron { get; }
    }
}