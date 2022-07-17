using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductFocus.Persistence.DbContexts;
using System;
using System.Data.Common;

namespace ProductFocusApi.IntegrationEvents.Services
{
    public class ProductFocusIntegrationEventLogService : IntegrationEventLogService, IProductFocusIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        private readonly DbConnection _dbConnection;
        private volatile bool disposedValue;

        public ProductFocusIntegrationEventLogService(DbConnection dbConnection, IConfiguration configuration)
            : base(configuration)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options, configuration);
        }

        public ProductFocusIntegrationEventLogService(ProductFocusIntegrationEventLogContext context, IConfiguration configuration)
            : base(configuration)
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
