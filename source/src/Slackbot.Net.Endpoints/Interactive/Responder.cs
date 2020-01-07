using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Slackbot.Net.Endpoints.Interactive
{
    public class Responder : IRespond
    {
        private readonly ILogger<Responder> _logger;

        public Responder(ILogger<Responder> logger)
        {
            _logger = logger;
        }

        public async Task<RespondResult> Respond(string responseUrl, string responseText)
        {
            var httpClient = new HttpClient();
            var response = new Acknowledge
            {
                Text = responseText,
                Is_Ephemeral = true
            };
            var serializedResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
            var content = new StringContent(serializedResponse, Encoding.UTF8, "application/json");
            var resp = await httpClient.PostAsync(responseUrl, content);
            var response_url_response = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogError("Could not reply to response_url");
                _logger.LogError(response_url_response);
                return new RespondResult {Success = false};
            }

            _logger.LogInformation(response_url_response);
            return new RespondResult {Success = true};
        }
    }
}