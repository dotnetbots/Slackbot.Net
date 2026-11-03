using System.Net;
using System.Text;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.SlackClients.Http.Models.Requests.FileUpload;
using Slackbot.Net.Tests.Helpers;

namespace Slackbot.Net.Tests;

public class FileUploadExternalTests(ITestOutputHelper helper)
{
    private readonly RoutingHttpMessageHandler _handler = new(new Dictionary<string, string>
    {
        ["https://slack.com/api/files.getUploadURLExternal"] = """{ "ok": true, "upload_url": "https://files.slack.com/upload/v1/abc", "file_id": "F123" }""",
        ["https://files.slack.com/upload/v1/abc"] = "OK - 5",
        ["https://slack.com/api/files.completeUploadExternal"] = """{ "ok": true, "files": [{ "id": "F123", "title": "beer" }] }""",
    });

    private ISlackClient Client => new SlackClient(
        new HttpClient(_handler) { BaseAddress = new Uri("https://slack.com/api/") },
        new XUnitLogger<ISlackClient>(helper));

    [Fact]
    public async Task FilesUpload_Bytes_GetsUrl_Uploads_ThenCompletes()
    {
        var response = await Client.FilesUpload(new FileUploadMultiPartRequest
        {
            Channels = "C123",
            File = [1, 2, 3, 4, 5],
            Filename = "beer.png",
            Title = "beer",
            Thread_Ts = "1.2"
        });

        Assert.Equal("F123", response.File.Id);
        Assert.Collection(_handler.Requests,
            r => Assert.Equal("filename=beer.png&length=5", r.FormBody),
            r => Assert.Equal(new byte[] { 1, 2, 3, 4, 5 }, r.Body),
            r => Assert.Equal("""files=[{"id":"F123","title":"beer"}]&channels=C123&thread_ts=1.2""", r.FormBody));
    }

    [Fact]
    public async Task FilesUpload_Content_UploadsUtf8_AndTitleFallsBackToFilename()
    {
        await Client.FilesUpload(new FileUploadRequest { Content = "hello", Filename = "hi.txt" });

        Assert.Equal("hello", Encoding.UTF8.GetString(_handler.Requests[1].Body));
        Assert.Equal("""files=[{"id":"F123","title":"hi.txt"}]""", _handler.Requests[2].FormBody);
    }

    private class RoutingHttpMessageHandler(Dictionary<string, string> responsesByUrl) : HttpMessageHandler
    {
        public List<(byte[] Body, string FormBody)> Requests { get; } = [];

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = await request.Content!.ReadAsByteArrayAsync(cancellationToken);
            Requests.Add((body, WebUtility.UrlDecode(Encoding.UTF8.GetString(body))));
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(responsesByUrl[request.RequestUri!.ToString()]) };
        }
    }
}
