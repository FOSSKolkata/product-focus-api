using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Releases.Application.Controllers;
using Releases.Domain.Common;
using Releases.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Releases.Application.StartUp
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddReleases(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddControllers()
                .PartManager
                .ApplicationParts.Add(new AssemblyPart(typeof(ReleaseController).Assembly));

            var connection = configuration["DefaultConnection"];
            services.AddDbContext<ReleaseDbContext>(
                            x => x.UseLazyLoadingProxies()
                                .UseSqlServer(connection));

            return services;
        }
    }
}
