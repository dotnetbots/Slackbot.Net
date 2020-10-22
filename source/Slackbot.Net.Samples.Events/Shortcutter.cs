using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Models.Events;

namespace CronBackgroundServices.Samples.Events
{
    public class Shortcutter : IShortcutHandler
    {
        private readonly IEnumerable<IHandleEvent> _handlers;
        

        public Shortcutter(IEnumerable<IHandleEvent> allHandlers)
        {
            _handlers = allHandlers;
        }

        public Task Handle(EventMetaData eventMetadata, SlackEvent @event)
        {
            var text = _handlers.Where(handler => handler.GetHelpDescription().HandlerTrigger != "donotshowinhelp")
                .Select(handler => handler.GetHelpDescription())
                .Aggregate("*HALP:*", (current, kvPair) => current + $"\n• `{kvPair.HandlerTrigger}` : _{kvPair.Description}_");
            
            
            Console.WriteLine(text);
            return Task.CompletedTask;
        }

        public bool ShouldHandle(SlackEvent @event)
        {
            if (@event is AppMentionEvent)
            {
                var appMention = (AppMentionEvent) @event;
                return appMention.Text.Contains("help");
            }

            return false;
        }
    }
}