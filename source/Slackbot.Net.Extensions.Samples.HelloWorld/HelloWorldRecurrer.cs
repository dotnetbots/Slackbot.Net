using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Extensions.Samples.HelloWorld
{
    internal class HelloWorldRecurrer : IRecurringAction
    {
        private readonly IPublisher _publisher;

        public HelloWorldRecurrer(IPublisher publisher)
        {
            _publisher = publisher;
        }
        
        public async Task Process()
        {
            await _publisher.Publish(new Notification
            {
                Msg = $"Hi"
            });
        }

        public string Cron { get; } = "*/10 * * * * *";
    }
}