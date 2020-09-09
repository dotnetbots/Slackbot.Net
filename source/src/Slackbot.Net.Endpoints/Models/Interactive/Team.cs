using Newtonsoft.Json;

namespace Slackbot.Net.Endpoints.Interactive
{
    public class Team
    {
        [JsonProperty("team_id")]
        public string Team_Id { get; set; }
    }
}