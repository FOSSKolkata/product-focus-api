using EventBus.Events;
using System.Threading.Tasks;

namespace Releases.Application.IntegrationEvents.Services
{
    public interface IReleaseIntegrationEventService
    {
        Task SaveEventAndVitalsContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
