namespace Slackbot.Net.Abstractions.Handlers
{
    public class HandleResponse
    {
        public HandleResponse(string message)
        {
            HandledMessage = message;
        }

        public string HandledMessage
        {
            get;
            set;
        }
    }
}