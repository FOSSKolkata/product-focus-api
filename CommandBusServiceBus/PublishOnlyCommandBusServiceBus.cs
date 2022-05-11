using Autofac;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using CommandBus.Abstractions;
using CommandBus;
using CommandBus.Commands;

namespace CommandBusServiceBus
{
    public class PublishOnlyCommandBusServiceBus<TOtherService> : IPublishOnlyCommandBus<TOtherService>
    where TOtherService : IOtherService
    {
        private readonly IServiceBusPersistentConnection _serviceBusPersisterConnection;
        private readonly ILogger<PublishOnlyCommandBusServiceBus<TOtherService>> _logger;
        private const string INTEGRATION_COMMAND_SUFFIX = "IntegrationCommand";

        public PublishOnlyCommandBusServiceBus(CommandBusConfiguration commandBusConfiguration, TOtherService otherService,
            ILogger<PublishOnlyCommandBusServiceBus<TOtherService>> logger)
        {
            OtherConnection otherConnection = commandBusConfiguration.OtherConnections.Where(x => x.Service == otherService.Name).Single();
            var serviceBusConnection = new ServiceBusConnectionStringBuilder(otherConnection.ConnectionString);
            _serviceBusPersisterConnection = new DefaultServiceBusPersisterConnection(serviceBusConnection);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishAsync(IntegrationCommand command)
        {
            var eventName = command.GetType().Name.Replace(INTEGRATION_COMMAND_SUFFIX, "");
            var jsonMessage = JsonSerializer.Serialize(command, command.GetType());
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };

            await _serviceBusPersisterConnection.QueueClient.SendAsync(message);
            //.GetAwaiter()
            //.GetResult();
        }
    }

    public class PublishOnlyCommandBusServiceBus<TOwningService, TOtherService> :
      PublishOnlyCommandBusServiceBus<TOtherService>,
      IPublishOnlyCommandBus<TOwningService, TOtherService>
         where TOwningService : IOwningService
         where TOtherService : IOtherService
    {
        public PublishOnlyCommandBusServiceBus(
            CommandBusConfiguration<TOwningService> commandBusConfiguration,
            TOtherService otherService,
            ILogger<PublishOnlyCommandBusServiceBus<TOwningService, TOtherService>> logger)
            : base(commandBusConfiguration, otherService, logger)
        {
        }
    }
}
