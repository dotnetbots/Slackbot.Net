using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Interactive;

namespace Slackbot.Net.Endpoints.Hosting
{
    public interface ISlackbotEndpointsBuilder
    {
        IServiceCollection Services { get; }
        ISlackbotEndpointsBuilder AddInteractiveHandler<T>() where T: class, IHandleInteractiveActions;
    }
}