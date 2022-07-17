using Microsoft.EntityFrameworkCore;
using CommandBus.Extensions;
using CommandBus.Commands;

namespace IntegrationCommandLogEF.Services
{
    public abstract class IncomingIntegrationCommandLogService : IIncomingIntegrationCommandLogService, IDisposable
    {
        protected abstract IntegrationCommandLogContext IntegrationCommandLogContext { get; }
        private readonly List<Type> _commandTypes;
        private volatile bool disposedValue;

        public IncomingIntegrationCommandLogService()
        {
            _commandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetLoadableTypes())
                .Where(t => t.Name.EndsWith(nameof(IntegrationCommand)))
                .ToList();
        }


        public async Task<IncomingIntegrationCommandLogEntry> RetrieveCommandLogAsync(Guid commandId)
        {

            var result = await IntegrationCommandLogContext.IncomingIntegrationCommandLogs
                .Where(e => e.CommandId == commandId && e.State == IncomingCommandStateEnum.ProcessingInProgress).SingleOrDefaultAsync();

            if (result != null)
            {
                return result.DeserializeJsonContent(_commandTypes.Find(t => t.Name == result.CommandTypeShortName));
            }

            return null;
        }

        public Task SaveAndMarkCommandAsInProgressAsync(IntegrationCommand command)
        {
            var commandLogEntry = new IncomingIntegrationCommandLogEntry(command, Guid.Empty);
            commandLogEntry.State = IncomingCommandStateEnum.ProcessingInProgress;
            IntegrationCommandLogContext.IncomingIntegrationCommandLogs.Add(commandLogEntry);

            return IntegrationCommandLogContext.SaveChangesAsync();
        }

        public Task MarkCommandAsProcessedAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, IncomingCommandStateEnum.Processed);
        }

        public Task MarkCommandAsFailedAsync(Guid commandId, Exception exception)
        {
            var commandLogEntry = IntegrationCommandLogContext.IncomingIntegrationCommandLogs.Single(ie => ie.CommandId == commandId);
            commandLogEntry.State = IncomingCommandStateEnum.ProcessingFailed;
            commandLogEntry.SetDiagnosticDetails(exception);

            IntegrationCommandLogContext.IncomingIntegrationCommandLogs.Update(commandLogEntry);

            return IntegrationCommandLogContext.SaveChangesAsync();
        }
        private Task UpdateCommandStatus(Guid commandId, IncomingCommandStateEnum status)
        {
            var commandLogEntry = IntegrationCommandLogContext.IncomingIntegrationCommandLogs.Single(ie => ie.CommandId == commandId);
            commandLogEntry.State = status;

            if (status == IncomingCommandStateEnum.ProcessingInProgress)
                commandLogEntry.TimesReceived++;

            IntegrationCommandLogContext.IncomingIntegrationCommandLogs.Update(commandLogEntry);

            return IntegrationCommandLogContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    IntegrationCommandLogContext?.Dispose();
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