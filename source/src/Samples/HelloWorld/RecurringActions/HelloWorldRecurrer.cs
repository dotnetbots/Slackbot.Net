using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CronBackgroundServices.Abstractions.Handlers;
using Microsoft.Extensions.Logging;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.SlackClients.Http;
using Slackbot.Net.SlackClients.Http.Models.Responses.UsersList;

namespace HelloWorld.RecurringActions
{
    internal class HelloWorldRecurrer : IRecurringAction
    {
        private readonly ISlackClientBuilder _clientService;
        private readonly ITokenStore _tokenStore;
        private readonly ILogger<HelloWorldRecurrer> _logger;

        public HelloWorldRecurrer(ISlackClientBuilder clientService, ITokenStore tokenStore, ILogger<HelloWorldRecurrer> logger)
        {
            _clientService = clientService;
            _tokenStore = tokenStore;
            _logger = logger;
        }
        
        public async Task Process(CancellationToken stoppingToken)
        {
            var allTokens = await _tokenStore.GetTokens();
            foreach (var token in allTokens)
            {
                var slackClient = _clientService.Build(token);
                var users = await slackClient.UsersList();
                if (users != null && users.Ok)
                {
                    var user = users.Members.FirstOrDefault(IsMySelf);
                    if (user != null)
                    {
                        await slackClient.ChatPostMessage(user.Id, $"Hi, @{user.Name}!");
                    }
                    else
                    {
                        _logger.LogInformation("No user found");
                    }  
                }
                else
                {
                    _logger.LogInformation(users.Error);
                }
            }
        }

        private bool IsMySelf(User u)
        {
            var realName = Contains(u.Real_name, "Korsnes");
            var user = Contains(u.Name, "johnkors");
            return realName || user;
        }

        private static bool Contains(string property, string contains)
        {
            return !string.IsNullOrEmpty(property) && property.Contains(contains);
        }

        public string Cron { get; } = "*/40 * * * * *";
    }
}