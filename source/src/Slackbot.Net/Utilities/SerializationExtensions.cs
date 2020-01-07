using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Slackbot.Net.Utilities
{
    public static class SerializationExtensions
    {
        public static string ToSerialized<T>(this T theObj)
        {
            return JsonConvert.SerializeObject(theObj, JsonSettings.SlackSettings);
        }
    }
    
    public class JsonSettings 
    {
        public static readonly JsonSerializerSettings SlackSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };
    }
}