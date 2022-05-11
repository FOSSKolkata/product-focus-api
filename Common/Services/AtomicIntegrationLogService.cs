using BuildingBlocks.IntegrationCommon.Utilities;
using CommandBus.Commands;
using EventBus.Events;
using IntegrationCommandLogEF.Services;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace IntegrationCommon.Services
{
    public class AtomicIntegrationLogService<TContext, TCommandLogService, TEventLogService> : IAtomicIntegrationLogService
        where TContext : DbContext
        where TCommandLogService : IIntegrationCommandLogService
        where TEventLogService : IIntegrationEventLogService
    {
        private readonly TContext _dbContext;
        private readonly TCommandLogService _commandLogService;
        private readonly TEventLogService _eventLogService;
        private readonly Func<DbConnection, TCommandLogService> _integrationCommandLogServiceFactory;
        private readonly Func<DbConnection, TEventLogService> _integrationEventLogServiceFactory;
        private readonly ILogger<AtomicIntegrationLogService<TContext, TCommandLogService, TEventLogService>> _logger;
        private readonly List<IntegrationCommand> _commands;
        private readonly List<IntegrationEvent> _events;
        public AtomicIntegrationLogService(TContext dbContext,
            Func<DbConnection, TCommandLogService> integrationCommandLogServiceFactory,
            Func<DbConnection, TEventLogService> integrationEventLogServiceFactory,
            ILogger<AtomicIntegrationLogService<TContext, TCommandLogService, TEventLogService>> logger)
        {
            _dbContext = dbContext;
            _integrationCommandLogServiceFactory = integrationCommandLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationCommandLogServiceFactory));
            _commandLogService = _integrationCommandLogServiceFactory(_dbContext.Database.GetDbConnection());
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventLogService = _integrationEventLogServiceFactory(_dbContext.Database.GetDbConnection()); ;
            _commands = new List<IntegrationCommand>();
            _events = new List<IntegrationEvent>();
            _logger = logger;
        }

        public AtomicIntegrationLogService<TContext, TCommandLogService, TEventLogService> AddCommand(IntegrationCommand command)
        {
            _commands.Add(command);
            return this;
        }

        public AtomicIntegrationLogService<TContext, TCommandLogService, TEventLogService> AddEvent(IntegrationEvent @event)
        {
            _events.Add(@event);
            return this;
        }


        public async Task SaveAtomicallyWithDbContextChangesAsync()
        {
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await new ResilientTransaction(_dbContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original catalog database operation, the IntegrationEventLog and
                // the IntegrationCommandLog thanks to a local transaction
                await _dbContext.SaveChangesAsync();

                foreach (var evt in _events)
                    await _eventLogService.SaveEventAsync(evt, _dbContext.Database.CurrentTransaction);

                foreach (var cmnd in _commands)
                    await _commandLogService.SaveCommandAsync(cmnd, _dbContext.Database.CurrentTransaction);
            });
        }
    }
}
