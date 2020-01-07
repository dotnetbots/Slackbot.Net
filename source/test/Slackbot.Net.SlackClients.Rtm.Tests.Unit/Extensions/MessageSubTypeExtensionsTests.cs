using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Extensions;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;
using MessageSubType = Slackbot.Net.SlackClients.Rtm.Models.MessageSubType;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.Extensions
{
    public class MessageSubTypeExtensionsTests
    {
        [Theory]
        [InlineData(Rtm.Connections.Sockets.Messages.Inbound.MessageSubType.bot_message, MessageSubType.BotMessage)]
        private void should_convert_to_expected_enum(Rtm.Connections.Sockets.Messages.Inbound.MessageSubType inbound, MessageSubType expected)
        {
            inbound
                .ToSlackMessageSubType()
                .ShouldBe(expected);
        }
    }
}