using System.Linq;
using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Models;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;
using Slackbot.Net.SlackClients.Rtm.Models;
using Xunit;
using User = Slackbot.Net.SlackClients.Rtm.Connections.Models.User;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.SlackConnectorTests
{
    public class ConnectionInfoMapperTests
    {
        [Fact]
        public void should_map_self()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id", Name = "my-name" },
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            // then
            connInfo.Self.Id.ShouldBe("my-id");
            connInfo.Self.Name.ShouldBe("my-name");
        }
        
        [Fact]
        public void should_map_team()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Team = new Detail { Id = "team-id", Name = "team-name" },
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            // then
            connInfo.Team.Id.ShouldBe("team-id");
            connInfo.Team.Name.ShouldBe("team-name");
        }
        
        [Fact]
        public void should_map_channels()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Channels = new[]
                {
                    new Channel { Id = "i-am-a-channel", Name = "channel-name" , IsMember = true, Members = new [] { "member1", "member2" }},
                    new Channel { Id = "i-am-another-channel", Name = "but-you-aint-invited" , IsMember = false },
                    new Channel { Id = "i-am-archived-channel", Name = "please-ignore-me" , IsMember = true, IsArchived = true },
                }
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            // then
            connInfo.SlackChatHubs.Count.ShouldBe(1);
            connInfo.SlackChatHubs["i-am-a-channel"].Id.ShouldBe("i-am-a-channel");
            connInfo.SlackChatHubs["i-am-a-channel"].Name.ShouldBe("#channel-name");
            connInfo.SlackChatHubs["i-am-a-channel"].Type.ShouldBe(ChatHubType.Channel);
            connInfo.SlackChatHubs["i-am-a-channel"].Members.ShouldBe(handshakeResponse.Channels[0].Members);
        }
        
        [Fact]
        public void should_map_groups()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id" },
                Groups = new[]
                {
                    new Group { Id = "i-am-a-group", Name = "group-name", Members = new [] {"my-id", "another-member"} },
                    new Group { Id = "i-am-another-group", Name = "and-you-aint-a-member-of-it", Members = null },
                    new Group { Id = "i-am-a-group", Name = "group-name", Members = new [] {"my-id"}, IsArchived = true },
                },
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);
            
            connInfo.SlackChatHubs.Count.ShouldBe(1);
            connInfo.SlackChatHubs["i-am-a-group"].Id.ShouldBe("i-am-a-group");
            connInfo.SlackChatHubs["i-am-a-group"].Name.ShouldBe("#group-name");
            connInfo.SlackChatHubs["i-am-a-group"].Type.ShouldBe(ChatHubType.Group);
            connInfo.SlackChatHubs["i-am-a-group"].Members.ShouldBe(handshakeResponse.Groups[0].Members);
        }
        
        [Fact]
        public void should_map_ims()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Self = new Detail { Id = "my-id" },
                Users = new[]
                {
                    new User { Id = "user-guid-thingy", Name = "expected-name" },
                },
                Ims = new[]
                {
                    new Im { Id = "i-am-a-im", User = "user-i-am_yup" },
                    new Im { Id = "user-with-name", User = "user-guid-thingy" },
                },
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            connInfo.SlackChatHubs.Count.ShouldBe(2);
            connInfo.SlackChatHubs["i-am-a-im"].Id.ShouldBe("i-am-a-im");
            connInfo.SlackChatHubs["i-am-a-im"].Name.ShouldBe("@user-i-am_yup");
            connInfo.SlackChatHubs["i-am-a-im"].Type.ShouldBe(ChatHubType.DM);
            connInfo.SlackChatHubs["user-with-name"].Id.ShouldBe("user-with-name");
            connInfo.SlackChatHubs["user-with-name"].Name.ShouldBe("@expected-name");
            connInfo.SlackChatHubs["user-with-name"].Type.ShouldBe(ChatHubType.DM);
        }
        
        [Fact]
        public void should_not_map_archived_channels()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Channels = new[]
                {
                    new Channel
                    {
                        Id = "Id1",
                        Name = "Name1",
                        IsArchived = true,
                        IsMember = true 
                    },
                    new Channel
                    {
                        Id = "Id2",
                        Name = "Name2",
                        IsArchived = true,
                        IsMember = true 
                    },
                }
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            connInfo.SlackChatHubs.Count.ShouldBe(0);
        }
        
        [Fact]
        public void should_not_map_channels_bot_is_not_a_member_of()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Channels = new[]
                {
                    new Channel
                    {
                        Id = "Id1",
                        Name = "Name1",
                        IsArchived = false,
                        IsMember = false
                    }
                }
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            connInfo.SlackChatHubs.Count.ShouldBe(0);
        }
        
        [Fact]
        public void should_not_contain_archived_groups()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Groups = new[]
                {
                    new Group
                    {
                        Id = "group-id",
                        Name = "group-name",
                        IsArchived = true
                    }
                }
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);

            connInfo.SlackChatHubs.Count.ShouldBe(0);
        }
        
        [Fact]
        public void should_contain_channel_and_username()
        {
            // given
            var handshakeResponse = new HandshakeResponse
            {
                Ok = true,
                Users = new[]
                {
                    new User
                    {
                        Id = "user-id-thingy",
                        Name = "name-4eva"
                    }
                },
                Ims = new[]
                {
                    new Im
                    {
                        Id = "im-id-yay",
                        User = "user-id-thingy"
                    }
                }
            };

            var connInfo = ConnectionInformationMapper.CreateConnectionInformation("meh", handshakeResponse);


            // then
            connInfo.SlackChatHubs.Count.ShouldBe(1);
            connInfo.SlackChatHubs.First().Value.Name.ShouldBe("@" + handshakeResponse.Users[0].Name);
        }
    }
}