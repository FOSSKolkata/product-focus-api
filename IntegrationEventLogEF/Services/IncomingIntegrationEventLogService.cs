using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EventBus.Extensions;
using EventBus.Events;

namespace IntegrationEventLogEF.Services
{


    public abstract class IncomingIntegrationEventLogService : IIncomingIntegrationEventLogService, IDisposable
    {
        protected abstract IntegrationEventLogContext IntegrationEventLogContext { get; }
        private readonly List<Type> _eventTypes;
        private volatile bool disposedValue;

        public IncomingIntegrationEventLogService()
        {
            _eventTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetLoadableTypes())
                .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                .ToList();
        }

       
        public async Task<IncomingIntegrationEventLogEntry> RetrieveEventLogAsync(Guid eventId)
        {

            var result = await IntegrationEventLogContext.IncomingIntegrationEventLogs
                .Where(e => e.EventId == eventId && e.State == IncomingEventStateEnum.ProcessingInProgress).SingleOrDefaultAsync();

            if (result != null)
            {
                return result.DeserializeJsonContent(_eventTypes.Find(t => t.Name == result.EventTypeShortName));
            }

            return null;
        }

        public Task SaveAndMarkEventAsInProgressAsync(IntegrationEvent @event)
        {
            var eventLogEntry = new IncomingIntegrationEventLogEntry(@event, Guid.Empty);
            eventLogEntry.State = IncomingEventStateEnum.ProcessingInProgress;
            IntegrationEventLogContext.IncomingIntegrationEventLogs.Add(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }

        public Task MarkEventAsProcessedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, IncomingEventStateEnum.Processed);
        }

        public Task MarkEventAsFailedAsync(Guid eventId, Exception exception)
        {
            var eventLogEntry = IntegrationEventLogContext.IncomingIntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = IncomingEventStateEnum.ProcessingFailed;
            eventLogEntry.SetDiagnosticDetails(exception);

            IntegrationEventLogContext.IncomingIntegrationEventLogs.Update(eventLogEntry);

            return IntegrationEventLogContext.SaveChangesAsync();
        }
        private Task UpdateEventStatus(Guid eventId, IncomingEventStateEnum status)
        {
            var eventLogEntry = IntegrationEventLogContext.IncomingIntegrationEventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if (status == IncomingEventStateEnum.ProcessingInProgress)
                eventLogEntry.TimesReceived++;

            IntegrationEventLogContext.IncomingIntegrationEventLogs.Update(eventLogEntry);

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
