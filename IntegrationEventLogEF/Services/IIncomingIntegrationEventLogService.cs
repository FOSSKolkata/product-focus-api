
using EventBus.Events;

namespace IntegrationEventLogEF.Services
{
    public interface IIncomingIntegrationEventLogService
    {
        Task<IncomingIntegrationEventLogEntry> RetrieveEventLogAsync(Guid eventId);
        Task SaveAndMarkEventAsInProgressAsync(IntegrationEvent @event);
        Task MarkEventAsProcessedAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId, Exception exception);
    }
}
