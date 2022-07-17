using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Releases.Infrastructure.DbContexts
{
    public class ReleaseContext : IntegrationEventLogContext
    {
        public ReleaseContext(DbContextOptions<ReleaseContext> options, IConfiguration configuration) : base(options, "release")
        {
        }
    }
}
