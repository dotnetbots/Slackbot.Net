using Newtonsoft.Json;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests
{
    public class ConversationsRepliesTests : Setup
    {
        private readonly ITestOutputHelper _helper;

        public ConversationsRepliesTests(ITestOutputHelper helper) : base(helper)
        {
            _helper = helper;
        }
        
        [Fact]
        public async Task ConversationsRepliesWorks()
        {
            var response = await SlackClient.ConversationsReplies("C0EC3DG5N","1679144061.148689");
            _helper.WriteLine(JsonConvert.SerializeObject(response));
            Assert.True(response.Ok);
        }
    }
}