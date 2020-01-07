using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Moq;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Monitoring;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Models;
using Slackbot.Net.SlackClients.Rtm.Tests.Unit.TestExtensions;
using Xunit;
using File = Slackbot.Net.SlackClients.Rtm.Models.File;
using MessageSubType = Slackbot.Net.SlackClients.Rtm.Models.MessageSubType;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.SlackConnectionTests.InboundMessageTests
{
    public class ChatMessageTests
    {
        [Theory, AutoMoqData]
        private async Task should_raise_event(
            Mock<IWebSocketClient> webSocket, 
            Mock<IPingPongMonitor> pingPongMonitor,
            Mock<IHandshakeClient> handShakeClient)
        {
            var slackConnection = new Connection(pingPongMonitor.Object, handShakeClient.Object, webSocket.Object);


            // given
            var connectionInfo = new ConnectionInformation
            {
                Users =
                {
                    { "userABC", new User { Id = "userABC", Name = "i-have-a-name" } }
                }
            };
            await slackConnection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                User = "userABC",
                MessageType = MessageType.Message,
                Text = "amazing-text",
                RawData = "I am raw data yo",
                MessageSubType = Rtm.Connections.Sockets.Messages.Inbound.MessageSubType.bot_message
            };

            Message receivedMessage = null;
            slackConnection.OnMessageReceived += message =>
            {
                receivedMessage = message;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            receivedMessage.ShouldLookLike(new Message
            {
                Text = "amazing-text",
                User = new User { Id = "userABC", Name = "i-have-a-name" },
                RawData = inboundMessage.RawData,
                MessageSubType = MessageSubType.BotMessage,
                Files = Enumerable.Empty<File>()
        });
        }

        [Theory, AutoMoqData]
        private async Task should_raise_event_given_user_information_is_missing_from_cache(
            Mock<IWebSocketClient> webSocket, 
            Mock<IHandshakeClient> handShakeClient,
            Mock<IPingPongMonitor> pingPongMonitor)
        {
            // given
            
            var slackConnection = new Connection(pingPongMonitor.Object, handShakeClient.Object, webSocket.Object);

            var connectionInfo = new ConnectionInformation();
            
            await slackConnection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                User = "userABC",
                MessageType = MessageType.Message
            };

            Message receivedMessage = null;
            slackConnection.OnMessageReceived += message =>
            {
                receivedMessage = message;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            receivedMessage.ShouldLookLike(new Message
            {
                User = new User { Id = "userABC", Name = string.Empty },
                Files = Enumerable.Empty<File>()
        });
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_message_event_given_incorrect_message_type(
            Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            var connectionInfo = new ConnectionInformation();
            await connection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage { MessageType = MessageType.Unknown };

            bool messageRaised = false;
            connection.OnMessageReceived += message =>
            {
                messageRaised = true;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            messageRaised.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_message_event_given_null_message(
            Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            var connectionInfo = new ConnectionInformation();
            await connection.Initialise(connectionInfo);

            bool messageRaised = false;
            connection.OnMessageReceived += message =>
            {
                messageRaised = true;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, null);

            // then
            messageRaised.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private async Task should_return_expected_channel_info(
            Mock<IWebSocketClient> webSocket, 
            Mock<IHandshakeClient> handShakeClient,
            Mock<IPingPongMonitor> pingPongMonitor)
        {
            // given
            
            var slackConnection = new Connection(pingPongMonitor.Object, handShakeClient.Object, webSocket.Object);

            var connectionInfo = new ConnectionInformation
            {
                SlackChatHubs = { { "channelId", new ChatHub { Id = "channelId", Name = "NaMe23" } } },
            };
            await slackConnection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                Channel = connectionInfo.SlackChatHubs.First().Key,
                MessageType = MessageType.Message,
                User = "irmBrady"
            };

            Message receivedMessage = null;
            slackConnection.OnMessageReceived += message =>
            {
                receivedMessage = message;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            ChatHub expected = connectionInfo.SlackChatHubs.First().Value;
            receivedMessage.ChatHub.ShouldBe(expected);
        }

        [Theory, AutoMoqData]
        private async Task should_detect_bot_is_mentioned_in_message(
            Mock<IWebSocketClient> webSocket,
            Mock<IHandshakeClient> handShakeClient,
            Mock<IPingPongMonitor> pingPongMonitor)
        {
            var slackConnection = new Connection(pingPongMonitor.Object, handShakeClient.Object, webSocket.Object);
            // given
            var connectionInfo = new ConnectionInformation
            {
                Self = new ContactDetails { Id = "self-id", Name = "self-name" },
            };
            await slackConnection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                MessageType = MessageType.Message,
                Text = "please @self-name, send help... :-p",
                User = "lalala"
            };

            Message receivedMessage = null;
            slackConnection.OnMessageReceived += message => { receivedMessage = message; return Task.CompletedTask; };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            receivedMessage.MentionsBot.ShouldBeTrue();
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_message_event_given_message_from_self(
            Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            var connectionInfo = new ConnectionInformation
            {
                Self = new ContactDetails { Id = "self-id", Name = "self-name" },
            };
            await connection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                MessageType = MessageType.Message,
                User = connectionInfo.Self.Id
            };

            bool messageRaised = false;
            connection.OnMessageReceived += message =>
            {
                messageRaised = true;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            messageRaised.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_event_given_message_is_missing_user_information(
            Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            var connectionInfo = new ConnectionInformation();
            await connection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                MessageType = MessageType.Message,
                User = null
            };

            bool messageRaised = false;
            connection.OnMessageReceived += message =>
            {
                messageRaised = true;
                return Task.CompletedTask;
            };

            // when
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);

            // then
            messageRaised.ShouldBeFalse();
        }

        [Theory, AutoMoqData]
        private async Task should_not_raise_exception(
            Mock<IWebSocketClient> webSocket, 
            Connection connection)
        {
            // given
            var connectionInfo = new ConnectionInformation();
            await connection.Initialise(connectionInfo);

            var inboundMessage = new ChatMessage
            {
                MessageType = MessageType.Message,
                User = "lalala"
            };

            connection.OnMessageReceived += message => throw new Exception("EMPORER OF THE WORLD");

            // when & then (does not throw)
            webSocket.Raise(x => x.OnMessage += null, null, inboundMessage);
        }
    }
}
