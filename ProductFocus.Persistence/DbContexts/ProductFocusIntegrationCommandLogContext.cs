using IntegrationCommandLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.DbContexts
{
    public class ProductFocusIntegrationCommandLogContext : IntegrationCommandLogContext
    {
        public ProductFocusIntegrationCommandLogContext(DbContextOptions<ProductFocusIntegrationCommandLogContext> options, IConfiguration configuration) : base(options, "dbo")
        {
        }
    }
}
