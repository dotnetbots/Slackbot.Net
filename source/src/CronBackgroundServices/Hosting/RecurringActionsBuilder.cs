using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CronBackgroundServices.Hosting
{
    public class RecurringActionsBuilder : IRecurringActionsBuilder
    {
        public RecurringActionsBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services
        {
            get;
        }

        public IRecurringActionsBuilder Build()
        {
            var recurrers = Services.Where(s => s.ServiceType == typeof(IRecurringAction)).ToList();
            if(!recurrers.Any())
                throw new Exception("No recurrers added. Missing");

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
        
        public IRecurringActionsBuilder AddRecurrer<T>() where T : class, IRecurringAction
        {
            Services.AddSingleton<IRecurringAction,T>();
            return this;
        }
    }
}