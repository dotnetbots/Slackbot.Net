using System.Collections;
using Slackbot.Net.Models.BlockKit;
using Slackbot.Net.SlackClients.Http.Exceptions;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostEphemeral;
using Slackbot.Net.SlackClients.Http.Models.Requests.ChatPostMessage;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class ChatPostMessageTests(ITestOutputHelper helper) : Setup(helper)
{
    [Fact]
    public async Task PostMinimalWorks()
    {
        var response = await SlackClient.ChatPostMessage(Channel, Text);
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task PostWorks()
    {
        var msg = new ChatPostMessageRequest { Channel = Channel, Text = Text };
        var response = await SlackClient.ChatPostMessage(msg);
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task PostEphemeralWorks()
    {
        var msg = new ChatPostEphemeralMessageRequest { Channel = Channel, Text = Text, User = "U0EBWMGG4" };
        var response = await SlackClient.ChatPostEphemeralMessage(msg);
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task PostEphemeralThreadWorks()
    {
        var initMsg = await SlackClient.ChatPostMessage(Channel, "Some thread starting text");
        var reply = await SlackClient.ChatPostMessage(new ChatPostMessageRequest
        {
            Channel = Channel, Text = "A threaded reply ", thread_ts = initMsg.ts
        });
        var msg = new ChatPostEphemeralMessageRequest
        {
            Channel = Channel, Text = "This is ephemeral to johnkors", User = "U0EBWMGG4", thread_ts = reply.ts
        };
        var response = await SlackClient.ChatPostEphemeralMessage(msg);
        Assert.True(response.Ok);
    }

    [Fact]
    public async Task PostWithBroadCastWorks()
    {
        var msg = new ChatPostMessageRequest
        {
            Channel = "CTECR3J6M", thread_ts = "1679186947.684479", Text = "BROADCAST!", Reply_Broadcast = true
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
            Channel = Channel, Text = Text + " and @jarlelin", Link_Names = link_names
        };
        var response = await SlackClient.ChatPostMessage(msg);
        Assert.True(response.Ok);
    }

    [Theory]
    [ClassData(typeof(AllBlocks))]
    public async Task PostBlocksWorks(IBlock blocks)
    {
        var msg = new ChatPostMessageRequest { Channel = Channel, Text = Text, Blocks = new[] { blocks } };
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
        yield return Scenario(new ImageBlock
        {
            image_url = "https://placehold.co/150", alt_text = "some alt text"
        });
        yield return Scenario(new InputBlock { label = TextElement(), element = new PlainTextInputElement() });
        yield return Scenario(new SectionBlock { text = TextElement() });

        object[] Scenario(IBlock block)
        {
            return new object[] { block };
        }

        IElement[] Elements(IElement element)
        {
            return new[] { element };
        }

        IElement ButtonElement()
        {
            return new ButtonElement { text = TextElement() };
        }

        Text TextElement()
        {
            return new Text { text = "TextElementText" };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
