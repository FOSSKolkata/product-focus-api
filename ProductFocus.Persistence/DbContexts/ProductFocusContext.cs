using IntegrationEventLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.DbContexts
{
    public class ProductFocusContext : IntegrationEventLogContext
    {
        public ProductFocusContext(DbContextOptions<ProductFocusContext> options, IConfiguration configuration) : base(options, "dbo")
        {
        }
    }
}
