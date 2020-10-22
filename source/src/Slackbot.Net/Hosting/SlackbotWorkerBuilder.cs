using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Dynamic;

namespace Slackbot.Net.Hosting
{
    public class SlackbotWorkerBuilder : ISlackbotWorkerBuilder
    {
        public SlackbotWorkerBuilder(IServiceCollection services)
        {
            services.AddSingleton<ISlackClientService, SlackClientService>();
            Services = services;
        }

        public IServiceCollection Services
        {
            get;
        }

        public ISlackbotWorkerBuilder BuildRecurrers()
        {
            var recurrers = Services.Where(s => s.ServiceType == typeof(IRecurringAction)).ToList();

            foreach (var recurrer in recurrers)
            {
                Services.AddTransient<IHostedService>(s =>
                {
                    var allRecurrers = s.GetServices<IRecurringAction>();
                    var single = allRecurrers.First(r => r.GetType() == recurrer.ImplementationType);
                    var loggerFactory = s.GetService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger(single.GetType());
                    return new CronBackgroundService(single,logger);
                }); 
            }

            return this;
        }
    }
}