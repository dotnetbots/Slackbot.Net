using System.Text.Json.Serialization;

namespace Slackbot.Net.Azure.Functions.Models.Interactive;

public class User
{
    [JsonPropertyName("user_id")]
    public string User_Id { get; set; }
}
