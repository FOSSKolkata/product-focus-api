using EventBus.Events;
using System.Threading.Tasks;

namespace ProductFocusApi.IntegrationEvents.Services
{
    public interface IProductFocusIntegrationEventService
    {
        Task SaveEventAndProductFocusDbContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
