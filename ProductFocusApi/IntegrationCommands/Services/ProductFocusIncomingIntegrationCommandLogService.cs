using IntegrationCommandLogEF;
using IntegrationCommandLogEF.Services;
using ProductFocus.Persistence.DbContexts;

namespace ProductFocusApi.IntegrationCommands.Services
{
    public class ProductFocusIncomingIntegrationCommandLogService : IncomingIntegrationCommandLogService, IProductFocusIncomingIntegrationCommandLogService
    {
        private readonly ProductFocusIntegrationCommandLogContext _integrationCommandLogContext;
        private volatile bool disposedValue;


        public ProductFocusIncomingIntegrationCommandLogService(ProductFocusIntegrationCommandLogContext context)
        {
            _integrationCommandLogContext = context;
        }

        protected override IntegrationCommandLogContext IntegrationCommandLogContext => _integrationCommandLogContext;
        protected override void Dispose(bool disposing)
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
    }
}