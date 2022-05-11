using EventBus.Events;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF.Services
{
    public interface IIntegrationEventLogService
    {
        Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId);
        Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsInProgressAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
        Task<IntegrationEventLogEntry> RetrieveEventLogAsync(Guid eventId);
        Task SaveAndMarkIncomingEventAsInProgressAsync(IntegrationEvent @event);
        Task MarkIncomingEventAsProcessedAsync(Guid eventId);
        Task MarkIncomingEventAsFailedAsync(Guid eventId, Exception exception);
    }
}
