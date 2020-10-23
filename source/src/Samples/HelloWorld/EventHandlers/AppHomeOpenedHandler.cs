using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace HelloWorld.EventHandlers
{
    public class AppHomeOpenedHandler : IHandleAppHomeOpened
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppHomeOpenedEvent payload)
        {
            string json = JsonConvert.SerializeObject(payload);
            Console.WriteLine(json);
            return Task.FromResult(new EventHandledResponse(json));
        }
    }
}