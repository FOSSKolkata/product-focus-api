using Autofac;
using CommandBus;
using CommandBus.Abstractions;
using CommandBus.Commands;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace CommandBusServiceBus
{
    public class CommandBusServiceBus<TOwningService> : CommandBusServiceBus, ICommandBus<TOwningService>
       where TOwningService : IOwningService
    {
        public CommandBusServiceBus(CommandBusConfiguration<TOwningService> commandBusConfiguration,
           ILogger<CommandBusServiceBus<TOwningService>> logger, ICommandBusSubscriptionsManager<TOwningService> subsManager, ILifetimeScope autofac)
            : base(commandBusConfiguration, logger, subsManager, autofac)
        {
        }
    }


    public class CommandBusServiceBus : ICommandBus
    {
        private readonly IServiceBusPersistentConnection _serviceBusPersisterConnection;
        private readonly ILogger<CommandBusServiceBus> _logger;
        private readonly ICommandBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "eshop_command_bus";
        private const string INTEGRATION_COMMAND_SUFFIX = "IntegrationCommand";

        public CommandBusServiceBus(CommandBusConfiguration commandBusConfiguration,
            ILogger<CommandBusServiceBus> logger, ICommandBusSubscriptionsManager subsManager, ILifetimeScope autofac)
        {

            var serviceBusConnection = new ServiceBusConnectionStringBuilder(commandBusConfiguration.MyConnectionString);
            _serviceBusPersisterConnection = new DefaultServiceBusPersisterConnection(serviceBusConnection);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryCommandBusSubscriptionsManager();
            _autofac = autofac;

            RegisterQueueClientMessageHandler();
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

        public void Subscribe<T, TH>()
           where T : IntegrationCommand
           where TH : IIntegrationCommandHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_COMMAND_SUFFIX, "");
            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationCommand

            where TH : IIntegrationCommandHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_COMMAND_SUFFIX, "");

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationCommandHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationCommandHandler
        {
            _logger.LogInformation("Unsubscribing from dynamic event {EventName}", eventName);

            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            _subsManager.Clear();
        }

        private void RegisterQueueClientMessageHandler()
        {
            _serviceBusPersisterConnection.QueueClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    Console.WriteLine("entering in RegisterSubscriptionClientMessageHandler");
                    var commandName = $"{message.Label}{INTEGRATION_COMMAND_SUFFIX}";
                    var messageData = Encoding.UTF8.GetString(message.Body);

                    // Complete the message so that it is not received again.
                    if (await ProcessCommand(commandName, messageData))
                    {
                        await _serviceBusPersisterConnection.QueueClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                    Console.WriteLine("exiting in RegisterSubscriptionClientMessageHandler");
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

            return Task.CompletedTask;
        }

        private async Task<bool> ProcessCommand(string commandName, string message)
        {
            var processed = false;
            if (_subsManager.HasSubscriptionsForCommand(commandName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subsManager.GetHandlersForCommand(commandName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationCommandHandler;
                            if (handler == null) continue;

                            using dynamic eventData = JsonDocument.Parse(message);
                            await handler.Handle(eventData);
                        }
                        else
                        {
                            var handler = scope.ResolveOptional(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetCommandTypeByName(commandName, subscription.HandlerType);
                            var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                            var concreteType = typeof(IIntegrationCommandHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
                processed = true;
            }
            return processed;
        }
    }
}
