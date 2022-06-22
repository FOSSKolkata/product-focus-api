using CommandBus;
using CommandBus.Abstractions;
using CommandBusRabbitMQ;
using CommandBusServiceBus;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using EventBusServiceBus;
using IntegrationCommon.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ProductFocusApi.StartUp
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddCommandBuses(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(AtomicIntegrationLogService<,,>));

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton(typeof(IPublishOnlyCommandBus<,>), typeof(PublishOnlyCommandBusServiceBus<,>));
                services.AddSingleton(typeof(ICommandBus<>), typeof(CommandBusServiceBus<>));
            }
            else
            {
                services.AddSingleton(typeof(IPublishOnlyCommandBus<,>), typeof(PublishOnlyCommandBusRabbitMQ<,>));
                services.AddSingleton(typeof(ICommandBus<>), typeof(CommandBusRabbitMQ<>));
            }

            services.AddSingleton(typeof(ICommandBusSubscriptionsManager<>), typeof(InMemoryCommandBusSubscriptionsManager<>));


            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("Service bus initialize");
            Console.WriteLine("Azure Service Bus - {0}", configuration.GetValue<bool>("AzureServiceBusEnabled"));

            services.AddSingleton(typeof(IEventBusSubscriptionsManager<>), typeof(InMemoryEventBusSubscriptionsManager<>));

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton(typeof(IEventBus<>), typeof(EventBusServiceBus<>));
            }
            else
            {
                services.AddSingleton(typeof(IEventBus<>), typeof(EventBusRabbitMQ<>));
            }

            return services;
        }
    }
}
