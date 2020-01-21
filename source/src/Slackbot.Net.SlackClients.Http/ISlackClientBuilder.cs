namespace Slackbot.Net.SlackClients.Http
{
    public interface ISlackClientBuilder
    {
        ISlackClient Build(string token);
    }
}