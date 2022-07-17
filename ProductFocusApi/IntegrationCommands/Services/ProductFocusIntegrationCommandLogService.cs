using CommandBus.Commands;
using IntegrationCommandLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using ProductFocus.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProductFocusApi.IntegrationCommands.Services
{
    public class ProductFocusIntegrationCommandLogService : IProductFocusIntegrationCommandLogService
    {
        private readonly ProductFocusIntegrationCommandLogContext _integrationCommandLogContext;
        private readonly DbConnection _dbConnection;
        private readonly List<Type> _eventTypes;
        private volatile bool disposedValue;
        public ProductFocusIntegrationCommandLogService(DbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationCommandLogContext = new ProductFocusIntegrationCommandLogContext(
                new DbContextOptionsBuilder<ProductFocusIntegrationCommandLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options, configuration);

            // TODO : the event types may need to fetched from all assemblies of app domain instead of just the entry assembly
            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(IntegrationCommand)))
                .ToList();
        }
        public Task MarkCommandAsFailedAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, CommandStateEnum.PublishedFailed);
        }

        public Task MarkCommandAsInProgressAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, CommandStateEnum.InProgress);
        }

        public Task MarkCommandAsPublishedAsync(Guid commandId)
        {
            return UpdateCommandStatus(commandId, CommandStateEnum.Published);
        }

        public async Task<IEnumerable<IntegrationCommandLogEntry>> RetrieveCommandLogsPendingToPublishAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            var result = await _integrationCommandLogContext.IntegrationCommandLogs
                .Where(e => e.TransactionId == tid && e.State == CommandStateEnum.NotPublished).ToListAsync();

            if (result != null && result.Any())
            {
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.CommandTypeShortName)));
            }

            return new List<IntegrationCommandLogEntry>();
        }

        public Task SaveCommandAsync(IntegrationCommand command, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLogEntry = new IntegrationCommandLogEntry(command, transaction.TransactionId);

            _integrationCommandLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _integrationCommandLogContext.IntegrationCommandLogs.Add(eventLogEntry);

            return _integrationCommandLogContext.SaveChangesAsync();
        }

        private Task UpdateCommandStatus(Guid commandId, CommandStateEnum status)
        {
            var eventLogEntry = _integrationCommandLogContext.IntegrationCommandLogs.Single(ie => ie.CommandId == commandId);
            eventLogEntry.State = status;

            if (status == CommandStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _integrationCommandLogContext.IntegrationCommandLogs.Update(eventLogEntry);

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

        public Task SaveCommandAsync(List<IntegrationCommand> commands, IDbContextTransaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
