using Microsoft.Extensions.DependencyInjection;

namespace Slackbot.Net.Endpoints.Hosting
{
    public interface ISlackbotEndpointsBuilder
    {
        IServiceCollection Services { get; }
    }
}