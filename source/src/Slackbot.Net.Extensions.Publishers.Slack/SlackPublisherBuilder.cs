using System.Threading.Tasks;
using Slackbot.Net.Abstractions.Hosting;
using Slackbot.Net.Abstractions.Publishers;
using Slackbot.Net.SlackClients.Http;

namespace Slackbot.Net.Extensions.Publishers.Slack
{
    public class SlackPublisherBuilder : IPublisherBuilder
    {
        private readonly ISlackClientBuilder _clientFactory;
        private readonly ITokenStore _tokenStore;

        public SlackPublisherBuilder(ISlackClientBuilder clientFactory, ITokenStore tokenStore)
        {
            _clientFactory = clientFactory;
            _tokenStore = tokenStore;
        }
        
        public async Task<IPublisher> Build(string slackTeamId)
        {
            var token = await _tokenStore.GetTokenByTeamId(slackTeamId);
            var slackClient = _clientFactory.Build(token);
            return new SlackPublisher(slackClient);
        }
    }
}