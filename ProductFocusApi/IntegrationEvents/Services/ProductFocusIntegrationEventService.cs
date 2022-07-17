using EventBus.Abstractions;
using EventBus.Events;
using IntegrationCommandLogEF.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductFocus.Persistence.DbContexts;
using ProductFocusApi.IntegrationCommands.Services;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace ProductFocusApi.IntegrationEvents.Services
{
    public class ProductFocusIntegrationEventService : IProductFocusIntegrationEventService, IDisposable
    {
        private readonly Func<DbConnection, IProductFocusIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus<ProductFocusEventBusOwningService> _eventBus;
        private readonly ProductFocusContext _productFocusContext;
        private readonly IProductFocusIntegrationEventLogService _eventLogService;
        private readonly ILogger<ProductFocusIntegrationEventService> _logger;
        private volatile bool disposedValue;
        private readonly string AppName = "ProductFocus Reading";
        public ProductFocusIntegrationEventService(
            ILogger<ProductFocusIntegrationEventService> logger,
            IEventBus<ProductFocusEventBusOwningService> eventBus,
            ProductFocusContext productFocusContext,
            Func<DbConnection, IProductFocusIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productFocusContext = productFocusContext ?? throw new ArgumentNullException(nameof(productFocusContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_productFocusContext.Database.GetDbConnection());
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

        public async Task SaveEventAndProductFocusContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- PatientStagesIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await new ResilientTransaction(_productFocusContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original catalog database operation and the IntegrationEventLog thanks to a local transaction
                await _productFocusContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _productFocusContext.Database.CurrentTransaction);
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
