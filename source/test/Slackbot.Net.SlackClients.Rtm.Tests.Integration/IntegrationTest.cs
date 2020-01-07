using System;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.Configurations;
using Slackbot.Net.SlackClients.Rtm.Tests.Integration.Configuration;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Integration
{
    public abstract class IntegrationTest : IDisposable
    {
        protected IConnection Connection;
        protected Config Config;

        protected IntegrationTest()
        {
            Config = new ConfigReader().GetConfig();

            var slackConnector = new Connector(new RtmOptions { ApiKey = Config.Slack.ApiToken});
            Connection = Task.Run(() => slackConnector.Connect())
                    .GetAwaiter()
                    .GetResult();
        }
        
        public virtual void Dispose()
        {
            Task.Run(() => Connection.Close())
                .GetAwaiter()
                .GetResult();
        }
    }
}