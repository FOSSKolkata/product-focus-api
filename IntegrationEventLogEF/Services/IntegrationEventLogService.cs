using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using EventBus.Extensions;
using Microsoft.Extensions.Configuration;
using EventBus.Events;

namespace IntegrationEventLogEF.Services
{


    public abstract class IntegrationEventLogService : IIntegrationEventLogService, IDisposable
    {
        protected abstract IntegrationEventLogContext IntegrationEventLogContext { get; }
        private readonly List<Type> _eventTypes;
        private volatile bool disposedValue;

        public IntegrationEventLogService(IConfiguration configuration)
        {
            _eventTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetLoadableTypes())
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await IntegrationEventLogContext.IntegrationEventLogs
                .Where(e => e.TransactionId == tid && e.State == EventStateEnum.NotPublished).ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)));
            }

            return new List<IntegrationEventLogEntry>();
        }

        public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLogEntry = new IntegrationEventLogEntry(@event, transaction.TransactionId);

            IntegrationEventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            IntegrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        public async Task<IntegrationEventLogEntry> RetrieveEventLogAsync(Guid eventId)
        {

            var result = await IntegrationEventLogContext.IntegrationEventLogs
                .Where(e => e.EventId == eventId && e.State == EventStateEnum.ProcessingInProgress).SingleOrDefaultAsync();

            if (result != null)
            {
                return result.DeserializeJsonContent(_eventTypes.Find(t => t.Name == result.EventTypeShortName));
            }

            return null;
        }

        public Task SaveAndMarkIncomingEventAsInProgressAsync(IntegrationEvent @event)
        {
            var eventLogEntry = new IntegrationEventLogEntry(@event, Guid.Empty);
            eventLogEntry.State = EventStateEnum.ProcessingInProgress;
            IntegrationEventLogContext.IntegrationEventLogs.Add(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkIncomingEventAsProcessedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Processed);
        }

        public Task MarkIncomingEventAsFailedAsync(Guid eventId, Exception exception)
        {
            var eventLogEntry = IntegrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = EventStateEnum.ProcessingFailed;
            eventLogEntry.SetDiagnosticDetails(exception);

            IntegrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }
        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = IntegrationEventLogContext.IntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            IntegrationEventLogContext.IntegrationEventLogs.Update(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    IntegrationEventLogContext?.Dispose();
                }


                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
