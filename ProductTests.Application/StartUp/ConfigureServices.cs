using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductTests.Controllers;
using ProductTests.Domain.Common;
using ProductTests.Infrastructure;

namespace ProductTests.Application.StartUp
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddTestManagement(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddControllers()
                .PartManager
                .ApplicationParts.Add(new AssemblyPart(typeof(TestCaseController).Assembly));

            var connection = configuration["DefaultConnection"];
            services.AddDbContext<ProductTestDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));
            return services;
        }
    }
}
