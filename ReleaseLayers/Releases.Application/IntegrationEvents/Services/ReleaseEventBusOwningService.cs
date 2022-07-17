using EventBus.Abstractions;

namespace Releases.Application.IntegrationEvents.Services
{
    public class ReleaseEventBusOwningService : IEventBusOwningService
    {
        public string Name => "Release";
    }
}
