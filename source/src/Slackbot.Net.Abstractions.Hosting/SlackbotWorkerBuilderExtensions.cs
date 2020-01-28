using System;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Abstractions.Handlers;
using Slackbot.Net.Abstractions.Publishers;

namespace Slackbot.Net.Abstractions.Hosting
{
    public static class SlackbotWorkerBuilderExtensions
    {
        public static ISlackbotWorkerBuilder AddHandler<T>(this ISlackbotWorkerBuilder builder) where T : class, IHandleMessages
        {
            builder.Services.AddSingleton<IHandleMessages, T>();
            return builder;
        }
        
        public static ISlackbotWorkerBuilder AddRecurring<T>(this ISlackbotWorkerBuilder builder) where T: class, IRecurringAction
        {
            builder.Services.AddSingleton<IRecurringAction, T>();
            return builder;
        }
        
        public static ISlackbotWorkerBuilder AddPublisher<T>(this ISlackbotWorkerBuilder builder) where T: class, IPublisher
        {
            if(typeof(T).Name == "SlackPublisher")
                throw new Exception("AddPublisher<SlackPublisher> is deprecated. Use `AddSlackPublisher` for standalone apps, or `AddSlackPublisherBuilder` for distributed apps");
            
            builder.Services.AddSingleton<IPublisher, T>();
            return builder;
        }

        public static ISlackbotWorkerBuilder AddPublisherFactory<T>(this ISlackbotWorkerBuilder builder) where T : class, IPublisherBuilder
        {
            builder.Services.AddSingleton<IPublisherBuilder, T>();
            return builder;
        }
    }
}