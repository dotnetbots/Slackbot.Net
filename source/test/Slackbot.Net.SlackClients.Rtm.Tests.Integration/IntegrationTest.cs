using System;
using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.Configurations;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Integration
{
    public abstract class IntegrationTest : IDisposable
    {
        protected IConnection Connection;
        protected string Token;

        protected IntegrationTest()
        {
            Token = Environment.GetEnvironmentVariable("Slackbot_SlackApiKey_BotUser");
            var slackConnector = new Connector(new RtmOptions { Token = Token});
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