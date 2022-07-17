using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProductFocus.Persistence.DbContexts
{
    public class ProductFocusIntegrationEventLogContext : IntegrationEventLogContext
    {
        public ProductFocusIntegrationEventLogContext(DbContextOptions<ProductFocusIntegrationEventLogContext> options,
            IConfiguration configuration) : base(options, "dbo")
        {
        }
    }
}
