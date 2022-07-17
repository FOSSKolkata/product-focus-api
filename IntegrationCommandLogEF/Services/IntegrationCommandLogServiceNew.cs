using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using CommandBus.Commands;
using CommandBus.Extensions;

namespace IntegrationCommandLogEF.Services
{
    public abstract class IntegrationCommandLogServiceNew : IIntegrationCommandLogService, IDisposable
    {
        protected abstract IntegrationCommandLogContext IntegrationCommandLogContext { get; }
        private readonly List<Type> _commandTypes;
        private volatile bool disposedValue;

        public IntegrationCommandLogServiceNew(IConfiguration configuration)
        {

            _commandTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetLoadableTypes())
                .Where(t => t.Name.EndsWith(nameof(IntegrationCommand)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationCommandLogEntry>> RetrieveCommandLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await IntegrationCommandLogContext.IntegrationCommandLogs
                .Where(e => e.TransactionId == tid && e.State == CommandStateEnum.NotPublished).ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_commandTypes.Find(t => t.Name == e.CommandTypeShortName)));
            }

            return new List<IntegrationCommandLogEntry>();
        }

        public Task SaveCommandAsync(IntegrationCommand @command, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var commandLogEntry = new IntegrationCommandLogEntry(@command, transaction.TransactionId);

            IntegrationCommandLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            IntegrationCommandLogContext.IntegrationCommandLogs.Add(commandLogEntry);

            return IntegrationCommandLogContext.SaveChangesAsync();
        }

        public Task SaveCommandAsync(List<IntegrationCommand> commands, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            IntegrationCommandLogContext.Database.UseTransaction(transaction.GetDbTransaction());

            foreach (var command in commands)
            {
                var commandLogEntry = new IntegrationCommandLogEntry(command, transaction.TransactionId);
                IntegrationCommandLogContext.IntegrationCommandLogs.Add(commandLogEntry);
            }

            return IntegrationCommandLogContext.SaveChangesAsync();
        }

        public Task MarkCommandAsPublishedAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, CommandStateEnum.Published);
        }

        public Task MarkCommandAsInProgressAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, CommandStateEnum.InProgress);
        }

        public Task MarkCommandAsFailedAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, CommandStateEnum.PublishedFailed);
        }

        private Task UpdateCommandStatus(Guid commandId, CommandStateEnum status)
        {
            var commandLogEntry = IntegrationCommandLogContext.IntegrationCommandLogs.Single(ie => ie.CommandId == commandId);
            commandLogEntry.State = status;

            if (status == CommandStateEnum.InProgress)
                commandLogEntry.TimesSent++;

            IntegrationCommandLogContext.IntegrationCommandLogs.Update(commandLogEntry);

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
