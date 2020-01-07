using System.ComponentModel.DataAnnotations;

namespace Slackbot.Net.Configuration
{
    public class SlackOptions
    {

        /// <summary>
        /// Only required if you call Slack APIs that only are for Bot Users, for example `search.messages`
        /// https://api.slack.com/methods/search.messages
        /// </summary>       
        public string Slackbot_SlackApiKey_SlackApp
        {
            get;
            set;
        }

        [Required]
        public string Slackbot_SlackApiKey_BotUser
        {
            get;
            set;
        }
    }
}