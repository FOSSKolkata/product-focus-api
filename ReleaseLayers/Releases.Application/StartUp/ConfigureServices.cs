using EventBus.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Releases.Application.Controllers;
using Releases.Application.IntegrationEvents.Services;
using Releases.Domain.Common;
using Releases.Infrastructure;
using Releases.Infrastructure.DbContexts;
using System.Data.Common;

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
            services.AddCustomDbContext(configuration)
                .AddCustomIntegrations(configuration);

            return services;
        }
        private static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration["QueriesConnectionString"];
            services.AddDbContext<ReleaseContext>(options =>
            {
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(connection,sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "release");
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
            });

            services.AddDbContext<ReleaseIntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(connection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFEventLogMigrationsHistory", "release");
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
             ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
           );
            services.AddDbContext<ReleaseIntegrationCommandLogContext>(options =>
            {
                options.UseSqlServer(connection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFCommandLogMigrationsHistory", "release");
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
            ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );

            return services;
        }
        private static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<Func<DbConnection, IReleaseIntegrationEventLogService>>(
             sp => (DbConnection c) => new ReleaseIntegrationEventLogService(c, configuration));

            services.AddTransient<IReleaseIntegrationEventLogService, ReleaseIntegrationEventLogService>();

            //services.AddTransient<Func<DbConnection, IReleaseIntegrationCommandLogService>>(
            //    sp => (DbConnection c) => new ReleaseIntegrationCommandLogService(c, configuration));

            //services.AddSingleton<ReleaseOwningService>();
            //services.AddSingleton<CommandBusConfiguration<ReleaseOwningService>>(sp =>
            //{
            //    List<CommandBusConfiguration<ReleaseOwningService>> commandBusConfigurations = configuration.GetSection("CommandBusConfigurations").Get<List<CommandBusConfiguration<ReleaseOwningService>>>();
            //    var owningService = services.BuildServiceProvider().GetService<ReleaseOwningService>();
            //    var commandBusConfiguration = commandBusConfigurations.Where(x => x.OwningService == owningService.Name).Single();
            //    return commandBusConfiguration;

            //});

            services.AddSingleton<EventBusConfiguration<ReleaseEventBusOwningService>>(sp =>
            {
                List<EventBusConfiguration<ReleaseEventBusOwningService>> eventBusConfigurations = configuration.GetSection("EventBusConfigurations").Get<List<EventBusConfiguration<ReleaseEventBusOwningService>>>();
                var owningService = new ReleaseEventBusOwningService();
                return eventBusConfigurations.Where(x => x.OwningService == owningService.Name).Single();
            });

            services.AddTransient<IReleaseIntegrationEventService, ReleaseIntegrationEventService>();
            services.AddTransient<IReleaseIncomingIntegrationEventLogService, ReleaseIncomingIntegrationEventLogService>();

            return services;
        }
    }
}
