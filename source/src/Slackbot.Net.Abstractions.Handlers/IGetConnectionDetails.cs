namespace Slackbot.Net.Abstractions.Handlers
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