using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.SlackClients;
using Slackbot.Net.SlackClients.Http;

namespace Slackbot.Net.Extensions.Publishers.Slack
{
    public class SlackPublisher : IPublisher
    {
        private readonly ISlackClient _sender;

        public SlackPublisher(ISlackClient sender)
        {
            _sender = sender;
        }

        public async Task Publish(Notification notification)
        {
            await _sender.ChatPostMessage(notification.Recipient, notification.Msg);
        }
    }
}