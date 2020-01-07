using System.Threading.Tasks;
using Slackbot.Net.SlackClients.Rtm.EventHandlers;
using Slackbot.Net.SlackClients.Rtm.Models;

namespace Slackbot.Net.SlackClients.Rtm
{
    public interface IConnection
    {
        /// <summary>
        /// Is the RealTimeConnection currently open?
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Connected Team Details.
        /// </summary>
        ContactDetails Team { get; }

        /// <summary>
        /// Authenticated Self Details.
        /// </summary>
        ContactDetails Self { get; }

        /// <summary>
        /// Close websocket connection to Slack
        /// </summary>
        Task Close();

        /// <summary>
        /// Raised when the websocket disconnects from the mothership.
        /// </summary>
        event DisconnectEventHandler OnDisconnect;

        /// <summary>
        /// Raised when attempting to reconnect to Slack after a disconnect is detected
        /// </summary>
        event ReconnectEventHandler OnReconnecting;

        /// <summary>
        /// Raised when a connection has been restored.
        /// </summary>
        event ReconnectEventHandler OnReconnect;

        /// <summary>
        /// Raised when real-time messages are received.
        /// </summary>
        event MessageReceivedEventHandler OnMessageReceived;

        /// <summary>
        /// Sends a Ping message to the server to detect if the client is disconnected
        /// </summary>
        /// <returns></returns>
        Task Ping();
        
        /// <summary>
        /// Slack Authentication Key.
        /// </summary>
        string SlackKey { get; }
    }
}