using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Releases.Infrastructure.DbContexts;
using System.Data.Common;

namespace Releases.Application.IntegrationEvents.Services
{
    public class ReleaseIntegrationEventLogService : IntegrationEventLogService, IReleaseIntegrationEventLogService
    {
        private readonly IntegrationEventLogContext _integrationEventLogContext;
        private readonly DbConnection _dbConnection;
        private volatile bool disposedValue;

        public ReleaseIntegrationEventLogService(DbConnection dbConnection, IConfiguration configuration)
            : base(configuration)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _integrationEventLogContext = new IntegrationEventLogContext(
                new DbContextOptionsBuilder<IntegrationEventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options, configuration);
        }

        public ReleaseIntegrationEventLogService(ReleaseIntegrationEventLogContext context, IConfiguration configuration)
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
