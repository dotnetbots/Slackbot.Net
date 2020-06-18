using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Interactive;

namespace Slackbot.Net.Endpoints.Hosting
{
    internal class SlackbotInteractiveEndpointsBuilder : ISlackbotEndpointsBuilder
    {
        public SlackbotInteractiveEndpointsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services
        {
            get;
        }
        
        public ISlackbotEndpointsBuilder AddInteractiveHandler<T>() where T: class, IHandleInteractiveActions
        {
            Services.AddSingleton<IHandleInteractiveActions, T>();
            return this;
        }

        internal ISlackbotEndpointsBuilder AddSlackbotInterctiveDependencies()
        {
            Services.AddSingleton<IRespond, Responder>();
            return this;
        }
    }
}