using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductDocumentations.Controllers;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Infrastructure;

namespace ProductDocumentations.Application.StartUp
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddProductDocumentation(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var connection = configuration["DefaultConnection"];
            services.AddDbContext<ProductDocumentationDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));

            services.AddControllers()
                .PartManager
                .ApplicationParts.Add(new AssemblyPart(typeof(ProductDocumentationController).Assembly));

            return services;
        }
    }
}
