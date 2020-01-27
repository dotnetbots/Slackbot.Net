namespace Slackbot.Net.SlackClients.Http
{
    public interface ISlackClientBuilder
    {
        ISlackClient BuildFromConfiguration();
        ISlackClient Build(string token);
    }
}