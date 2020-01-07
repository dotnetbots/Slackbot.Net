using Microsoft.Extensions.DependencyInjection;

namespace Slackbot.Net.Endpoints.Hosting
{
    internal class SlackbotEndpointsBuilder : ISlackbotEndpointsBuilder
    {
        public SlackbotEndpointsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services
        {
            get;
        }
    }
}