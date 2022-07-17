using IntegrationCommandLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Infrastructure.DbContexts
{
    public class ReleaseIntegrationCommandLogContext : IntegrationCommandLogContext
    {
        public ReleaseIntegrationCommandLogContext(DbContextOptions<ReleaseIntegrationCommandLogContext> options, IConfiguration configuration) : base(options, "release")
        {
        }
    }
}
