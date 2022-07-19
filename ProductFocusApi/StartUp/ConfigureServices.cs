using CommandBus;
using EventBus.Abstractions;
using IntegrationCommon.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductFocus.ConnectionString;
using ProductFocus.Domain.Common;
using ProductFocus.Persistence;
using ProductFocus.Persistence.DbContexts;
using ProductFocus.Services;
using ProductFocusApi.IntegrationCommands.Services;
using ProductFocusApi.IntegrationCommands.Services.Own;
using ProductFocusApi.IntegrationEvents.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using ProductFocusEventBusOwningService = ProductFocusApi.IntegrationEvents.Services.ProductFocusEventBusOwningService;

namespace ProductFocusApi.StartUp
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddProductFocus(this IServiceCollection services, IConfiguration configuration)
        {

            var connection = configuration["DefaultConnection"];

            services.AddDbContext<ProductFocusDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));

            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var queryBuilder = new SqlConnectionStringBuilder(
                configuration.GetConnectionString("QueriesConnectionString"));
            var queryConnection = configuration["QueriesConnectionString"];

            var queriesConnectionString = new QueriesConnectionString(queryConnection);
            services.AddSingleton(queriesConnectionString);
            services.AddTransient<IEmailService, EmailService>();

            services.AddCustomDbContext(configuration)
                .AddCustomIntegrations(configuration);

            services.AddTransient(typeof(AtomicIntegrationLogService<,,>));

            return services;
        }

        private static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration["QueriesConnectionString"];
            services.AddDbContext<ProductFocusDbContext>(options =>
            {
                options
                .UseLazyLoadingProxies()
                .UseSqlServer(connection,sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo");
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
            });

            services.AddDbContext<ProductFocusIntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(connection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFEventLogMigrationsHistory", "dbo");
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
             ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
           );
            services.AddDbContext<ProductFocusIntegrationCommandLogContext>(options =>
            {
                options.UseSqlServer(connection,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFCommandLogMigrationsHistory", "dbo");
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            },
            ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
            );

            return services;
        }
        private static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductFocusIncomingIntegrationCommandLogService, ProductFocusIncomingIntegrationCommandLogService>();

            services.AddTransient<Func<DbConnection, ProductFocusIntegrationCommandLogService>>(
                sp => (DbConnection c) => new ProductFocusIntegrationCommandLogService(c, configuration));

            services.AddSingleton<ProductFocusCommandBusOwningService>();
            services.AddSingleton<CommandBusConfiguration<ProductFocusCommandBusOwningService>>(sp =>
            {
                List<CommandBusConfiguration<ProductFocusCommandBusOwningService>> commandBusConfigurations = configuration.GetSection("CommandBusConfigurations").Get<List<CommandBusConfiguration<ProductFocusCommandBusOwningService>>>();
                var owningService = services.BuildServiceProvider().GetService<ProductFocusCommandBusOwningService>();
                var commandBusConfiguration = commandBusConfigurations.Where(x => x.OwningService == owningService.Name).Single();
                return commandBusConfiguration;

            });


            services.AddSingleton<EventBusConfiguration<ProductFocusEventBusOwningService>>(sp =>
            {
                List<EventBusConfiguration<ProductFocusEventBusOwningService>> eventBusConfigurations = configuration.GetSection("EventBusConfigurations").Get<List<EventBusConfiguration<ProductFocusEventBusOwningService>>>();
                var owningService = new ProductFocusEventBusOwningService();
                return eventBusConfigurations.Where(x => x.OwningService == owningService.Name).Single();
            });

            services.AddTransient<IProductFocusIntegrationEventService, ProductFocusIntegrationEventService>();
            services.AddTransient<IProductFocusIntegrationEventLogService, ProductFocusIntegrationEventLogService>();
            services.AddTransient<Func<DbConnection, ProductFocusIntegrationEventLogService>>(
             sp => (DbConnection c) => new ProductFocusIntegrationEventLogService(c, configuration));

            services.AddTransient<IProductFocusIncomingIntegrationEventLogService, ProductFocusIncomingIntegrationEventLogService>();

            return services;
        }
        
    }
}
