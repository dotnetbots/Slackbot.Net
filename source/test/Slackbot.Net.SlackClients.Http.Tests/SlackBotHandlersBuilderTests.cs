using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Slackbot.Net.Endpoints.Abstractions;
using Slackbot.Net.Endpoints.Hosting;
using Slackbot.Net.Endpoints.Models.Events;
using Slackbot.Net.Endpoints.Models.Interactive.BlockActions;
using Slackbot.Net.Endpoints.Models.Interactive.MessageActions;
using Slackbot.Net.Endpoints.Models.Interactive.ViewSubmissions;

namespace Slackbot.Net.Tests;

public class SlackBotHandlersBuilderTests
{
    [Fact]
    public void HandlerRegistrationsAreScoped()
    {
        var services = new ServiceCollection();

        services.AddSlackBotEvents()
            .AddAppMentionHandler<TestHandlers>()
            .AddMemberJoinedChannelHandler<TestHandlers>()
            .AddViewSubmissionHandler<TestHandlers>()
            .AddInteractiveBlockActionsHandler<TestHandlers>()
            .AddAppHomeOpenedHandler<TestHandlers>()
            .AddShortcut<TestShortcut>()
            .AddNoOpAppMentionHandler<TestNoOpAppMentions>()
            .AddMessageActionsHandler<TestHandlers>()
            .AddTeamJoinHandler<TestHandlers>()
            .AddEmojiChangedHandler<TestHandlers>()
            .AddMessageHandler<TestHandlers>()
            .AddReactionAddedHandler<TestHandlers>()
            .AddAssistantThreadStartedHandler<TestHandlers>()
            .AddAssistantThreadContextChangedHandler<TestHandlers>();

        AssertScoped<IHandleAppMentions, TestHandlers>(services);
        AssertScoped<IHandleMemberJoinedChannel, TestHandlers>(services);
        AssertScoped<IHandleViewSubmissions, TestHandlers>(services);
        AssertScoped<IHandleInteractiveBlockActions, TestHandlers>(services);
        AssertScoped<IHandleAppHomeOpened, TestHandlers>(services);
        AssertScoped<IShortcutAppMentions, TestShortcut>(services);
        AssertScoped<INoOpAppMentions, TestNoOpAppMentions>(services);
        AssertScoped<IHandleMessageActions, TestHandlers>(services);
        AssertScoped<IHandleTeamJoin, TestHandlers>(services);
        AssertScoped<IHandleEmojiChanged, TestHandlers>(services);
        AssertScoped<IHandleMessage, TestHandlers>(services);
        AssertScoped<IHandleReactionAdded, TestHandlers>(services);
        AssertScoped<IHandleAssistantThreadStarted, TestHandlers>(services);
        AssertScoped<IHandleAssistantThreadContextChanged, TestHandlers>(services);
    }

    [Fact]
    public async Task AppMentionSelectorResolvesHandlersFromCurrentRequestScope()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddScoped<ScopedDependency>();
        services.AddSlackBotEvents().AddAppMentionHandler<ScopedAppMentionHandler>();

        await using var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions
        {
            ValidateScopes = true
        });
        var selector = serviceProvider.GetRequiredService<ISelectAppMentionEventHandlers>();
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();

        await using var scope = serviceProvider.CreateAsyncScope();
        httpContextAccessor.HttpContext = new DefaultHttpContext
        {
            RequestServices = scope.ServiceProvider
        };

        var handlers = await selector.GetAppMentionEventHandlerFor(new EventMetaData(), new AppMentionEvent());

        var handler = Assert.IsType<ScopedAppMentionHandler>(Assert.Single(handlers));
        Assert.Same(scope.ServiceProvider.GetRequiredService<ScopedDependency>(), handler.Dependency);
    }

    private static void AssertScoped<TService, TImplementation>(IServiceCollection services)
    {
        var descriptor = Assert.Single(services, service =>
            service.ServiceType == typeof(TService) && service.ImplementationType == typeof(TImplementation));
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    private sealed class ScopedDependency;

    private sealed class ScopedAppMentionHandler(ScopedDependency dependency) : IHandleAppMentions
    {
        public ScopedDependency Dependency { get; } = dependency;

        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
        {
            return Task.FromResult(new EventHandledResponse("OK"));
        }
    }

    private sealed class TestNoOpAppMentions : INoOpAppMentions
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent)
        {
            return Task.FromResult(new EventHandledResponse("OK"));
        }
    }

    private sealed class TestShortcut : IShortcutAppMentions
    {
        public Task Handle(EventMetaData eventMetadata, AppMentionEvent @event)
        {
            return Task.CompletedTask;
        }

        public bool ShouldShortcut(AppMentionEvent @event)
        {
            return false;
        }
    }

    private sealed class TestHandlers :
        IHandleAppMentions,
        IHandleMemberJoinedChannel,
        IHandleViewSubmissions,
        IHandleInteractiveBlockActions,
        IHandleAppHomeOpened,
        IHandleMessageActions,
        IHandleTeamJoin,
        IHandleEmojiChanged,
        IHandleMessage,
        IHandleReactionAdded,
        IHandleAssistantThreadStarted,
        IHandleAssistantThreadContextChanged
    {
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppMentionEvent slackEvent) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MemberJoinedChannelEvent memberjoined) => Response();
        public Task<EventHandledResponse> Handle(ViewSubmission payload) => Response();
        public Task<EventHandledResponse> Handle(BlockActionInteraction blockActionEvent) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AppHomeOpenedEvent payload) => Response();
        public Task<EventHandledResponse> Handle(MessageActionInteraction blockActionEvent) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, TeamJoinEvent teamJoined) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, EmojiChangedEvent emojiChanged) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, MessageEvent appHomeMessage) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, ReactionAddedEvent slackEvent) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AssistantThreadStartedEvent slackEvent) => Response();
        public Task<EventHandledResponse> Handle(EventMetaData eventMetadata, AssistantThreadContextChangedEvent slackEvent) => Response();

        private static Task<EventHandledResponse> Response()
        {
            return Task.FromResult(new EventHandledResponse("OK"));
        }
    }
}
