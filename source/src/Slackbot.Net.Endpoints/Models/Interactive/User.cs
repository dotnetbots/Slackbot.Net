using System.Text.Json.Serialization;

namespace Slackbot.Net.Endpoints.Models.Interactive;

public class User
{
    [JsonPropertyName("id")]
    public string User_Id { get; set; }
}
