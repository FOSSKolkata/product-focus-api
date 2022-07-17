using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using ProductFocus.Persistence.DbContexts;

namespace ProductFocusApi.IntegrationEvents.Services
{
    public class ProductFocusIncomingIntegrationEventLogService : IncomingIntegrationEventLogService, IProductFocusIncomingIntegrationEventLogService
    {
        private readonly ProductFocusIntegrationEventLogContext _integrationEventLogContext;
        private volatile bool disposedValue;
        public ProductFocusIncomingIntegrationEventLogService(ProductFocusIntegrationEventLogContext context)
        {
            _integrationEventLogContext = context;
        }

        protected override IntegrationEventLogContext IntegrationEventLogContext => _integrationEventLogContext;
        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _integrationEventLogContext?.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}
