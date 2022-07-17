using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Releases.Infrastructure.DbContexts;

namespace Releases.Application.IntegrationEvents.Services
{
    public class ReleaseIncomingIntegrationEventLogService : IncomingIntegrationEventLogService, IReleaseIncomingIntegrationEventLogService
    {
        private readonly ReleaseIntegrationEventLogContext _integrationEventLogContext;
        private volatile bool disposedValue;
        public ReleaseIncomingIntegrationEventLogService(ReleaseIntegrationEventLogContext context)
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
