﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;
using Xunit.Abstractions;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Integration
{
    public class SlackConnectorTests : IntegrationTest
    {
        private readonly ITestOutputHelper _helper;

        public SlackConnectorTests(ITestOutputHelper helper)
        {
            _helper = helper;
        }
        
        [Fact]
        public async Task should_connect_and_stuff()
        {
            // given

            // when
            Connection.OnDisconnect += ConnectorOnDisconnect;
            Connection.OnMessageReceived += ConnectorOnMessageReceived;

            // then
            Connection.IsConnected.ShouldBeTrue();
            Thread.Sleep(TimeSpan.FromMinutes(5));

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
            _helper.WriteLine(message.Text);
            return Task.CompletedTask;
        }
    }
}