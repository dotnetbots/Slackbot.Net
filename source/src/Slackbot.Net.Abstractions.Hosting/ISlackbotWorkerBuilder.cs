using Microsoft.Extensions.DependencyInjection;

namespace Slackbot.Net.Abstractions.Hosting
{
    public interface ISlackbotWorkerBuilder
    {
        IServiceCollection Services { get; }
        void BuildRecurrers();
    }
}