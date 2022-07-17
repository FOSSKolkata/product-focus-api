using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using CommandBus.Extensions;
using CommandBus.Commands;

namespace IntegrationCommandLogEF.Services
{
    public class IntegrationCommandLogService : IIntegrationCommandLogService, IDisposable
    {
        private readonly IntegrationCommandLogContext _integrationCommandLogContext;
        private readonly DbConnection _dbConnection;
        private readonly List<Type> _commandTypes;
        private volatile bool disposedValue;

        public IntegrationCommandLogService(DbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationCommandLogContext = new IntegrationCommandLogContext(
                new DbContextOptionsBuilder<IntegrationCommandLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options, configuration);

            // TODO : the command types may need to fetched from all assemblies of app domain instead of just the entry assembly
            _commandTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetLoadableTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationCommand)))
                .ToList();
        }

        public async Task<IEnumerable<IntegrationCommandLogEntry>> RetrieveCommandLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await _integrationCommandLogContext.IntegrationCommandLogs
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

            _integrationCommandLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _integrationCommandLogContext.IntegrationCommandLogs.Add(commandLogEntry);

            return _integrationCommandLogContext.SaveChangesAsync();
        }
        public Task SaveCommandAsync(List<IntegrationCommand> commands, IDbContextTransaction transaction)
        {
            throw new NotImplementedException();
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
            var commandLogEntry = _integrationCommandLogContext.IntegrationCommandLogs.Single(ie => ie.CommandId == commandId);
            commandLogEntry.State = status;

            if (status == CommandStateEnum.InProgress)
                commandLogEntry.TimesSent++;

            _integrationCommandLogContext.IntegrationCommandLogs.Update(commandLogEntry);

            return _integrationCommandLogContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _integrationCommandLogContext?.Dispose();
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
