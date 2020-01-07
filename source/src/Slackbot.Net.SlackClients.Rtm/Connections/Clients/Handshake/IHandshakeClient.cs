using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.Connections.Responses;

namespace Slackbot.Net.SlackClients.Rtm.Connections.Clients.Handshake
{
    internal interface IHandshakeClient
    {
        /// <summary>
        /// No one likes a limp shake - AMIRITE?
        /// </summary>
        Task<HandshakeResponse> FirmShake(string slackKey);
    }
}