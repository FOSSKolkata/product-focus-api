using CommandBus.Commands;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace IntegrationCommandLogEF
{
    public class IncomingIntegrationCommandLogEntry
    {
        private IncomingIntegrationCommandLogEntry() { }
        public IncomingIntegrationCommandLogEntry(IntegrationCommand command, Guid transactionId)
        {
            CommandId = command.Id;
            CreationTime = command.CreationDate;
            CommandTypeName = command.GetType().FullName;
            Content = JsonSerializer.Serialize(command, command.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });
            State = IncomingCommandStateEnum.ProcessingInProgress;
            TimesReceived = 1;
        }

        public Guid CommandId { get; private set; }
        public string CommandTypeName { get; private set; }
        [NotMapped]
        public string CommandTypeShortName => CommandTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationCommand IntegrationCommand { get; private set; }
        public IncomingCommandStateEnum State { get; set; }
        public int TimesReceived { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string ExceptionMessage { get; private set; }
        public string StackTrace { get; private set; }

        public IncomingIntegrationCommandLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationCommand = JsonSerializer.Deserialize(Content, type, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) as IntegrationCommand;
            return this;
        }

        public void SetDiagnosticDetails(Exception exception)
        {
            this.ExceptionMessage = exception.Message + (exception.InnerException != null ? "; " + exception.InnerException.Message : string.Empty);
            this.StackTrace = exception.StackTrace;
        }
    }
}
