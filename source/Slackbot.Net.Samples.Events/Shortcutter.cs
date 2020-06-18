using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models;

namespace Slackbot.Net.Samples.Events
{
    public class Shortcutter : IShortcutHandler
    {
        private readonly IEnumerable<IHandleEvent> _handlers;
        

        public Shortcutter(IEnumerable<IHandleEvent> allHandlers)
        {
            _handlers = allHandlers;
        }

        public async Task Handle(EventMetaData eventMetadata, SlackEvent @event)
        {
            var text = _handlers.Where(handler => handler.ShouldShowInHelp)
                .Select(handler => handler.GetHelpDescription())
                .Aggregate("*HALP:*", (current, helpDescription) => current + $"\n• `{helpDescription.Item1}` : _{helpDescription.Item2}_");
            
            
            Console.WriteLine(text);   
        }

        public bool ShouldHandle(SlackEvent @event)
        {
            if (@event is AppMentionEvent)
            {
                var appMention = (AppMentionEvent) @event;
                return appMention.Text.Contains("help");
            }

            return true;
        }
    }
}