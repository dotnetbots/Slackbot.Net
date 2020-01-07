using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;
using Slackbot.Net.SlackClients.Rtm.Exceptions;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake
{
    internal class HandshakeClient : IHandshakeClient
    {
        private readonly HttpClient _httpClient;

        public HandshakeClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HandshakeResponse> FirmShake(string slackKey)
        {
            var uri = $"https://slack.com/api/rtm.start?token={slackKey}";
            var httpResponse = await _httpClient.GetAsync(uri);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<HandshakeResponse>(content);
            if (!response.Ok)
            {
                throw new CommunicationException($"Error occured while posting message '{response.Error}'");
            }
            return response;
        }
    }
}