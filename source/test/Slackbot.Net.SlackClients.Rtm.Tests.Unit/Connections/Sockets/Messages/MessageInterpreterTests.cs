using Shouldly;
using Slackbot.Net.SlackClients.Rtm.Connections.Sockets.Messages.Inbound;
using Slackbot.Net.SlackClients.Rtm.Tests.Unit.TestExtensions;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.Connections.Sockets.Messages
{
    public class MessageInterpreterTests
    {
        [Theory, AutoMoqData]
        private void should_return_standard_message(MessageInterpreter interpreter)
        {
            // given
            string json = @"
                {
                  'type': 'message',
                  'channel': '&lt;myChannel&gt;',
                  'user': '&lt;myUser&gt;',
                  'text': 'hi, my name is &lt;noobot&gt;',
                  'ts': '1445366603.000002',
                  'team': '&lt;myTeam&gt;'
                }
            ";

            // when
            var result = interpreter.InterpretMessage(json);

            // then
            var expected = new ChatMessage
            {
                MessageType = MessageType.Message,
                Channel = "<myChannel>",
                User = "<myUser>",
                Text = "hi, my name is <noobot>",
                Team = "<myTeam>",
                RawData = json,
                Timestamp = 1445366603.000002
            };

            result.ShouldLookLike(expected);
        }

        [Theory, AutoMoqData]
        private void should_return_message_with_file(MessageInterpreter interpreter)
        {
            // given
            string json = @"
                {
                  'type': 'message',
                  'files': [
                     {
                      'id':'some-id',
                      'created':12345,
                      'timestamp':54321,
                      'name':'name.gif',
                      'title':'title.gif',
                      'mimetype':'image\/gif',
                      'filetype':'gif',
                      'pretty_type':'GIF',
                      'user':'some-user',
                      'editable':true,
                      'size':63689,
                      'mode':'hosted',
                      'is_external':true,
                      'external_type':'some-external-type',
                      'is_public':true,
                      'public_url_shared':true,
                      'display_as_bot':true,
                      'username':'some-username',
                      'url_private':'https:\/\/url_private',
                      'url_private_download':'https:\/\/url_private_download',
                      'thumb_64':'https:\/\/thumb_64',
                      'thumb_80':'https:\/\/thumb_80',
                      'thumb_360':'https:\/\/thumb_360',
                      'thumb_360_w':43,
                      'thumb_360_h':29,
                      'thumb_160':'https:\/\/thumb_160',
                      'thumb_360_gif':'https:\/\/thumb_360_gif',
                      'image_exif_rotation':6,
                      'original_w':53,
                      'original_h':39,
                      'deanimate_gif':'https:\/\/deanimate_gif',
                      'permalink':'https:\/\/permalink',
                      'permalink_public':'https:\/\/permalink_public'
                   }]
                }
            ";

            // when
            var result = interpreter.InterpretMessage(json);

            // then
            var expected = new ChatMessage
            {
                MessageType = MessageType.Message,
                Files = new[] 
                {
                    new File
                    {
                        Id = "some-id",
                        Created = 12345,
                        Timestamp = 54321,
                        Name = "name.gif",
                        Title = "title.gif",
                        Mimetype = "image/gif",
                        FileType = "gif",
                        PrettyType = "GIF",
                        User = "some-user",
                        Editable = true,
                        Size = 63689,
                        Mode = "hosted",
                        IsExternal = true,
                        ExternalType = "some-external-type",
                        IsPublic = true,
                        PublicUrlShared = true,
                        DisplayAsBot = true,
                        Username = "some-username",
                        UrlPrivate = "https://url_private",
                        UrlPrivateDownload = "https://url_private_download",
                        Thumb64 = "https://thumb_64",
                        Thumb80 = "https://thumb_80",
                        Thumb360 = "https://thumb_360",
                        Thumb360Width = 43,
                        Thumb360Height = 29,
                        Thumb160 = "https://thumb_160",
                        Thumb360Gif = "https://thumb_360_gif",
                        ImageExifRotation = 6,
                        OriginalWidth = 53,
                        OriginalHeight = 39,
                        DeanimateGif = "https://deanimate_gif",
                        Permalink = "https://permalink",
                        PermalinkPublic = "https://permalink_public"
                    }
                },
                RawData = json
            };

            result.ShouldLookLike(expected);
        }

        [Theory, AutoMoqData]
        private void should_return_unknown_message_type(MessageInterpreter interpreter)
        {
            // given
            string json = @"{ 'type': 'something_else' }";

            // when
            var result = interpreter.InterpretMessage(json);

            // then
            var expected = new UnknownMessage
            {
                RawData = json
            };

            result.ShouldLookLike(expected);
        }

        [Theory, AutoMoqData]
        private void should_return_unknown_message_given_dodge_json(MessageInterpreter interpreter)
        {
            // given
            string json = @"{ 'type': 'something_else', 'channel': { 'isObject': true } }";

            // when
            var result = interpreter.InterpretMessage(json);

            // then
            result.ShouldBeOfType<UnknownMessage>();
        }

        [Theory, AutoMoqData]
        private void should_return_message_given_standard_message_with_null_data(MessageInterpreter interpreter)
        {
            // given
            string json = @"
                {
                  'type': 'message',
                  'channel': null,
                  'user': null,
                  'text': null,
                  'ts': '1445366603.000002',
                  'team': null
                }
            ";

            // when
            var result = interpreter.InterpretMessage(json);

            // then
            var expected = new ChatMessage
            {
                MessageType = MessageType.Message,
                Channel = null,
                User = null,
                Text = null,
                Team = null,
                RawData = json,
                Timestamp = 1445366603.000002
            };

            result.ShouldLookLike(expected);
        }

        [Theory, AutoMoqData]
        private void should_return_pong_message(MessageInterpreter interpreter)
        {
            // given
            string json = @"
                {
                  'type': 'pong',
                  'ts': '1445366603.000002'
                }
            ";

            // when
            var result = interpreter.InterpretMessage(json);

            // then
            var expected = new PongMessage
            {
                MessageType = MessageType.Pong,
                RawData = json
            };

            result.ShouldLookLike(expected);
        }
    }
}