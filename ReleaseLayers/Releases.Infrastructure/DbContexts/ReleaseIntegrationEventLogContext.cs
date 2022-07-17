using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Releases.Infrastructure.DbContexts
{
    public class ReleaseIntegrationEventLogContext : IntegrationEventLogContext
    {
        public ReleaseIntegrationEventLogContext(DbContextOptions<ReleaseIntegrationEventLogContext> options,
            IConfiguration configuration) : base(options, "release")
        {
        }
    }
}
