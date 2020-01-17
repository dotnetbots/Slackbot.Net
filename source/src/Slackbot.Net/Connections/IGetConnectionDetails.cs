using Slackbot.Net.Abstractions.Handlers;

namespace Slackbot.Net.Connections
{
    public interface IGetConnectionDetails
    {
        /// <summary>
        /// Throws NotConnectedException if connection is not connected
        /// </summary>
        /// <returns></returns>
        BotDetails GetConnectionBotDetails();
    }
}