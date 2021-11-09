using Slackbot.Net.Models.BlockKit;
using Slackbot.Net.SlackClients.Http.Models.Requests.ViewPublish;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests
{
    public class ViewPublishTests: Setup
    {
        public ViewPublishTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact(Skip = "not enabled")]
        public async Task ViewPublishWorks()
        {
            var response = await SlackClient.ViewPublish(new ViewPublishRequest("U0EBWMGG4")
            {
                View = new View
                {
                    Type = PublishViewConstants.Home,
                    Blocks = new IBlock[]
                    {
                        new SectionBlock()
                        {
                            text = new Text
                            {
                                text = "This is some other text"
                            }
                        }
                    }
                }
            });
            Assert.True(response.Ok);
        }
    }
}