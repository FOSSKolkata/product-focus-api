using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Releases.Application.IntegrationEvents.Events;
using Releases.Application.IntegrationEvents.Services;
using static Releases.Application.IntegrationEvents.Events.OrganizationAddedIntegrationEvent;

namespace Releases.Application.StartUp
{
    public static class ConfigureApp
    {
        public static IApplicationBuilder AddReleases(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus<ReleaseEventBusOwningService>>();
            eventBus.Subscribe<OrganizationAddedIntegrationEvent, OrganizationAddedIntegrationEventHandler>();
            return app;
        }
    }
}
