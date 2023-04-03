using Slackbot.Net.SlackClients.Http.Models.Requests.FileUpload;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests
{
    public class FileUploadTests : Setup
    {
        public FileUploadTests(ITestOutputHelper helper) : base(helper)
        {
        }
        
        [Fact]
        public async Task FilesUploadTests()
        {
            var response = await SlackClient.FilesUpload(new FileUploadRequest
            {
                Channels = $"{Channel}",
                Title = "Man in field",
                Initial_Comment = "My initial comment!",
                Content = "https://assets3.thrillist.com/v1/image/1682388/size/tl-horizontal_main.jpg",
                Filename = "heisann.jpg",
                Filetype = "jpg"
            });
            
            Assert.True(response.Ok);
        }
        
        [Fact]
        public async Task FilesUploadFileTests()
        {
            var bytes = Convert.FromBase64String(File.ReadAllText("./Helpers/ImageBase64Encoded.txt"));
            var response = await SlackClient.FilesUpload(new FileUploadMultiPartRequest
            {
                Channels = $"{Channel}",
                Title = "Man holding beer",
                File = bytes,
                Filename = "beer.png",
                Filetype = "png"
            });
            
            Assert.True(response.Ok);
        }
    }
}