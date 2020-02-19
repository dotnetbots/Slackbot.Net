using Slackbot.Net.SlackClients.Rtm;

namespace Slackbot.Net
{
    internal class ConnectedWorkspace
    {
        public string Token { get; set; }

        public string TeamId { get; set; }

        public string TeamName { get; set; }
        public IConnection Connection { get; set; }
    }
}