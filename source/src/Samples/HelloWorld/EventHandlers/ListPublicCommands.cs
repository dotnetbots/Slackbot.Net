using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace HelloWorld.EventHandlers
{
    public class ListPublicCommands : IShortcutAppMentions
    {
        private readonly IEnumerable<IHandleAppMentions> _handlers;
        

        public ListPublicCommands(IEnumerable<IHandleAppMentions> allHandlers)
        {
            _handlers = allHandlers;
        }

        public Task Handle(EventMetaData eventMetadata, AppMentionEvent @event)
        {
            var publicHandlersToBeListedBack = _handlers.Where(handler => handler.GetHelpDescription().HandlerTrigger != string.Empty)
    
                .Aggregate("*Public trigger mentions*", (current, kvPair) => current + $"\n• `{kvPair.GetType()}. `{kvPair.GetHelpDescription().HandlerTrigger}` : _{kvPair.GetHelpDescription().Description}_");
            
            
            Console.WriteLine(publicHandlersToBeListedBack);
            
            
            var ssh = _handlers.Where(handler => handler.GetHelpDescription().HandlerTrigger == string.Empty)
                .Aggregate("*Private trigger handlers*", (current, kvPair) => current + $"\n• `{kvPair.GetType()}`");
            Console.WriteLine(ssh);

            return Task.CompletedTask;
        }

        public bool ShouldShortcut(AppMentionEvent appMention) => appMention.Text.Contains("help");
    }
}