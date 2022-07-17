using EventBus.Events;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace IntegrationEventLogEF
{
    public class IncomingIntegrationEventLogEntry
    {

        private IncomingIntegrationEventLogEntry() { }
        public IncomingIntegrationEventLogEntry(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });
            State = IncomingEventStateEnum.ProcessingInProgress;
            TimesReceived = 1;
          }

        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationEvent IntegrationEvent { get; private set; }
        public IncomingEventStateEnum State { get; set; }
        public int TimesReceived { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string ExceptionMessage { get; private set; }
        public string StackTrace { get; private set; }

        public IncomingIntegrationEventLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationEvent = JsonSerializer.Deserialize(Content, type, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) as IntegrationEvent;
            return this;
        }

        public void SetDiagnosticDetails(Exception exception)
        {
            this.ExceptionMessage = exception.Message + (exception.InnerException != null ? "; " + exception.InnerException.Message : string.Empty);
            this.StackTrace = exception.StackTrace;
        }
    }
}
