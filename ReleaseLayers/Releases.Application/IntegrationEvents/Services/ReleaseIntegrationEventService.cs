using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Releases.Infrastructure.DbContexts;
using System;
using System.Data.Common;

namespace Releases.Application.IntegrationEvents.Services
{
    public class ReleaseIntegrationEventService : IReleaseIntegrationEventService
    {
        private readonly Func<DbConnection, IReleaseIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus<ReleaseEventBusOwningService> _eventBus;
        private readonly ReleaseContext _releaseContext;
        private readonly IReleaseIntegrationEventLogService _eventLogService;
        private readonly ILogger<ReleaseIntegrationEventService> _logger;
        private volatile bool disposedValue;
        private readonly string AppName = "ProductFocus Reading";
        public ReleaseIntegrationEventService(
            ILogger<ReleaseIntegrationEventService> logger,
            IEventBus<ReleaseEventBusOwningService> eventBus,
            ReleaseContext releaseContext,
            Func<DbConnection, IReleaseIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _releaseContext = releaseContext ?? throw new ArgumentNullException(nameof(releaseContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_releaseContext.Database.GetDbConnection());
        }
        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, AppName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                await _eventBus.PublishAsync(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, AppName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndVitalsContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- PatientStagesIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await new ResilientTransaction(_releaseContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                await _releaseContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _releaseContext.Database.CurrentTransaction);
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    (_eventLogService as IDisposable)?.Dispose();
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
