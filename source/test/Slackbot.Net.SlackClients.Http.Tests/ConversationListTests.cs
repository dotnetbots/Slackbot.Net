using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests
{
    public class ConversationsListTests : Setup
    {
        public ConversationsListTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact]
        public async Task ConversationsListWorks()
        {
            var response = await SlackClient.ConversationsListPublicChannels(300);
            Assert.True(response.Ok);
        }
        
        [Fact]
        public async Task ConversationsListsGeneral()
        {
            var response = await SlackClient.ConversationsListPublicChannels();
            var general = response.Channels.First(c => c.Is_General);
            Assert.NotNull(general.Id);
            Assert.NotNull(general.Name);
        }
        
        [Fact]
        public async Task ConversationsListsPublicChannels()
        {
            var response = await SlackClient.ConversationsListPublicChannels();
            var allResultsAreChannels = response.Channels.All(c => c.Is_Channel);
            Assert.True(allResultsAreChannels);
        }
        
        [Fact]
        public async Task ConversationsDoesNotIncludeArchivedChannels()
        {
            var response = await SlackClient.ConversationsListPublicChannels();
            var anyArchivedChannel  = response.Channels.Any(c => c.Is_Archived);
            Assert.False(anyArchivedChannel);
        }
        
        [Fact]
        public async Task ConversationsDoesNotIncludeDms()
        {
            var response = await SlackClient.ConversationsListPublicChannels();
            var anyDm  = response.Channels.Any(c => c.Is_Im);
            Assert.False(anyDm);
        }
        
        [Fact]
        public async Task ConversationsDoesNotIncludeGroup()
        {
            var response = await SlackClient.ConversationsListPublicChannels();
            var anyDm  = response.Channels.Any(c => c.Is_Group);
            Assert.False(anyDm);
        }
    }
}