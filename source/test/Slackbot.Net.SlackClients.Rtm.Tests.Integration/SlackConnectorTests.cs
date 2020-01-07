using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Integration
{
    public class SlackConnectorTests : IntegrationTest
    {
        [Fact]
        public async Task should_connect_and_stuff()
        {
            // given

            // when
            Connection.OnDisconnect += ConnectorOnDisconnect;
            Connection.OnMessageReceived += ConnectorOnMessageReceived;

            // then
            Connection.IsConnected.ShouldBeTrue();
            //Thread.Sleep(TimeSpan.FromMinutes(5));

            // when
            await Connection.Close();

            Connection.IsConnected.ShouldBeFalse();
        }

        private void ConnectorOnDisconnect()
        {

        }

        private Task ConnectorOnMessageReceived(Message message)
        {
            Debug.WriteLine(message.Text);
            Console.WriteLine(message.Text);
            return Task.CompletedTask;
        }
    }
}