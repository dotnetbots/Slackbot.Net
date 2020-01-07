using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExpectedObjects;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;
using Xunit;

namespace Slackbot.Net.SlackClients.Rtm.Tests.Unit.Connections.Clients.HandShake
{
    public class HandshakeClientTests
    {
        private readonly HandshakeClient _handshakeClient;
        private readonly Mock<HttpMessageHandler> _handlerMock;

        public HandshakeClientTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _handshakeClient = new HandshakeClient(new HttpClient(_handlerMock.Object));
        }

        [Fact]
        public async Task should_call_expected_url_with_given_slack_key()
        {
            const string slackKey = "I-is-da-key-yeah";

            var expectedResponse = new HandshakeResponse
            {
                Ok = true,
                WebSocketUrl = "some-url"
            };
           
            Setup(expectedResponse);
            var result = await _handshakeClient.FirmShake(slackKey);

            var expectedUri = new Uri($"https://slack.com/api/rtm.start?token={slackKey}");
            
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), 
                ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get  
                        && req.RequestUri == expectedUri 
                ),
                ItExpr.IsAny<CancellationToken>()
            );


            result.ToExpectedObject().ShouldEqual(expectedResponse);
        }

        private void Setup(HandshakeResponse expectedResponse)
        {
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(expectedResponse)),
                })
                .Verifiable();
        }
    }
}