using System.Collections;
using Slackbot.Net.Models.BlockKit;
using Slackbot.Net.SlackClients.Http.Exceptions;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class ChatPostMessageTests : Setup
{

    public ChatPostMessageTests(ITestOutputHelper helper) : base(helper)
    {
            
    }
        
    [Fact]
    public async Task PostMinimalWorks()
    {
        var response = await SlackClient.ChatPostMessage(Channel, Text);
        Assert.True(response.Ok);
    }
        
    [Fact]
    public async Task PostWorks()
    {
        var msg = new ChatPostMessageRequest
        {
            Channel = Channel,
            Text = Text
        };
        var response = await SlackClient.ChatPostMessage(msg);
        Assert.True(response.Ok);
    }
        
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task LinkNameWorks(bool link_names)
    {
        var msg = new ChatPostMessageRequest
        {
            Channel = Channel,
            Text = Text + " and @jarlelin",
            Link_Names = link_names
        };
        var response = await SlackClient.ChatPostMessage(msg);
        Assert.True(response.Ok);
    }
        
    [Theory]
    [ClassData(typeof(AllBlocks))]
    public async Task PostBlocksWorks(IBlock blocks)
    {
        var msg = new ChatPostMessageRequest
        {
            Channel = Channel,
            Text = Text,
            Blocks = new []{ blocks }
        };
        var response = await SlackClient.ChatPostMessage(msg);
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task PostMissingChannelThrowsSlackApiException()
    {
        await Assert.ThrowsAsync<WellKnownSlackApiException>(() => SlackClient.ChatPostMessage("", Text));
    }
}

public class AllBlocks : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return Scenario(new ActionsBlock { elements = Elements(ButtonElement()) });
        yield return Scenario(new ContextBlock { elements = Elements(TextElement()) });
        yield return Scenario(new DividerBlock());
        yield return Scenario(new ImageBlock { image_url = "https://via.placeholder.com/150", alt_text = "some alt text"});
        yield return Scenario(new InputBlock { label = TextElement(), element = new PlainTextInputElement()});
        yield return Scenario(new SectionBlock { text = TextElement()});

        object[] Scenario(IBlock block)
        {
            return new object[]
            {
                block
            };
        }

        IElement[] Elements(IElement element)
        {
            return new [] { element };
        }

        IElement ButtonElement()
        {
            return new ButtonElement { text = TextElement()};
        }
            
        Text TextElement()
        {
            return new Text { text = "TextElementText" };
        }
    }
        
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


}