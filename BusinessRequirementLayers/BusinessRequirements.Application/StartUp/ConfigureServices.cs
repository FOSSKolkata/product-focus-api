using Azure.Storage.Blobs;
using BusinessRequirements.Application.Controllers;
using BusinessRequirements.ConnectionString;
using BusinessRequirements.Domain.Common;
using BusinessRequirements.Domain.Services;
using BusinessRequirements.Infrastructure;
using BusinessRequirements.Persistence.Services;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessRequirements.Application.StartUp
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddBusinessRequirement(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            var blobConnection = configuration.GetConnectionString("BlobConnectionString");
            var businessRequirementAttachmentContainerName = configuration.GetConnectionString("BusinessRequirementAttachmentContainerName");
            services.AddSingleton(new BlobConnectionString(blobConnection));
            services.AddSingleton(new BlobServiceClient(blobConnection));
            services.AddSingleton(new BusinessRequirementContainerName(businessRequirementAttachmentContainerName));


            services.AddControllers()
                .PartManager
                .ApplicationParts.Add(new AssemblyPart(typeof(BusinessRequirementController).Assembly));
            var connection = configuration["DefaultConnection"];
            services.AddDbContext<BusinessRequirementDbContext>(
                            x => x.UseLazyLoadingProxies()
                                .UseSqlServer(connection));
            return services;
        }
    }
}
