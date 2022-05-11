using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;
using CommandBus.Commands;

namespace IntegrationCommandLogEF
{
    public class IntegrationCommandLogEntry
    {
        private IntegrationCommandLogEntry() { }
        public IntegrationCommandLogEntry(IntegrationCommand command, Guid transactionId)
        {
            CommandId = command.Id;
            CreationTime = command.CreationDate;
            CommandTypeName = command.GetType().FullName;
            Content = JsonSerializer.Serialize(command, command.GetType(), new JsonSerializerOptions
            {
                WriteIndented = true
            });
            State = CommandStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }
        public Guid CommandId { get; private set; }
        public string CommandTypeName { get; private set; }
        [NotMapped]
        public string CommandTypeShortName => CommandTypeName.Split('.')?.Last();
        [NotMapped]
        public IntegrationCommand IntegrationCommand { get; private set; }
        public CommandStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public IntegrationCommandLogEntry DeserializeJsonContent(Type type)
        {
            IntegrationCommand = JsonSerializer.Deserialize(Content, type, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) as IntegrationCommand;
            return this;
        }
    }
}
