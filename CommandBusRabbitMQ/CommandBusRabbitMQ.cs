using Autofac;
using CommandBus;
using CommandBus.Abstractions;
using CommandBus.Commands;
using CommandBus.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace CommandBusRabbitMQ
{

    public class CommandBusRabbitMQ : ICommandBus, IDisposable
    {
        const string AUTOFAC_SCOPE_NAME = "intellih_command_bus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<CommandBusRabbitMQ> _logger;
        private readonly ICommandBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private string _brokerName;

        private readonly int _retryCount;

        private IModel _consumerChannel;
        private string _queueName;

        public CommandBusRabbitMQ(CommandBusConfiguration commandBusConfiguration,
            ILogger<DefaultRabbitMQPersistentConnection> clogger,
            ILogger<CommandBusRabbitMQ> logger,
            ILifetimeScope autofac,
            ICommandBusSubscriptionsManager subsManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryCommandBusSubscriptionsManager();
            _brokerName = commandBusConfiguration.OwningService + "_exchange";
            _queueName = commandBusConfiguration.OwningService + "_queue";
            _autofac = autofac;
            _retryCount = 5;
            _subsManager.OnCommandRemoved += SubsManager_OnCommandRemoved;

            var factory = new ConnectionFactory()
            {
                HostName = commandBusConfiguration.MyConnectionString, // configuration["EventBusConnection"],
                DispatchConsumersAsync = true
            };

            _persistentConnection = new DefaultRabbitMQPersistentConnection(factory, clogger, _retryCount);
            _consumerChannel = CreateConsumerChannel();
        }

        private void SubsManager_OnCommandRemoved(object sender, string commandName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: _brokerName,
                    routingKey: commandName);

                if (_subsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public Task PublishAsync(IntegrationCommand command)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "Could not publish command: {CommandId} after {Timeout}s ({ExceptionMessage})", command.Id, $"{time.TotalSeconds:n1}", ex.Message);
                });

            var commandName = command.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish command: {CommandId} ({CommandName})", command.Id, commandName);

            using (var channel = _persistentConnection.CreateModel())
            {
                _logger.LogTrace("Declaring RabbitMQ exchange to publish command: {CommandId}", command.Id);

                channel.ExchangeDeclare(exchange: _brokerName, type: "direct");

                var body = JsonSerializer.SerializeToUtf8Bytes(command, command.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2; // persistent

                    _logger.LogTrace("Publishing command to RabbitMQ: {CommandId}", command.Id);

                    channel.BasicPublish(
                        exchange: _brokerName,
                        routingKey: commandName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                });
            }

            return Task.CompletedTask;
        }

        public void SubscribeDynamic<TH>(string commandName)
            where TH : IDynamicIntegrationCommandHandler
        {
            _logger.LogInformation("Subscribing to dynamic command {CommandName} with {CommandHandler}", commandName, typeof(TH).GetGenericTypeName());

            DoInternalSubscription(commandName);
            _subsManager.AddDynamicSubscription<TH>(commandName);
            StartBasicConsume();
        }

        public void Subscribe<T, TH>()
            where T : IntegrationCommand
            where TH : IIntegrationCommandHandler<T>
        {
            var commandName = _subsManager.GetCommandKey<T>();
            DoInternalSubscription(commandName);

            _logger.LogInformation("Subscribing to command {CommandName} with {CommandHandler}", commandName, typeof(TH).GetGenericTypeName());

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string commandName)
        {
            var containsKey = _subsManager.HasSubscriptionsForCommand(commandName);
            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: _queueName,
                                      exchange: _brokerName,
                                      routingKey: commandName);
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationCommand
            where TH : IIntegrationCommandHandler<T>
        {
            var commandName = _subsManager.GetCommandKey<T>();

            _logger.LogInformation("Unsubscribing from command {CommandName}", commandName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string commandName)
            where TH : IDynamicIntegrationCommandHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(commandName);
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }

        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs commandArgs)
        {
            var commandName = commandArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(commandArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessCommand(commandName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(commandArgs.DeliveryTag, multiple: false);
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _brokerName,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private async Task ProcessCommand(string commandName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ command: {CommandName}", commandName);

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
                            using dynamic commandData = JsonDocument.Parse(message);
                            await Task.Yield();
                            await handler.Handle(commandData);
                        }
                        else
                        {
                            try
                            {
                                var handler = scope.ResolveOptional(subscription.HandlerType);
                                if (handler == null) continue;
                                var commandType = _subsManager.GetCommandTypeByName(commandName, subscription.HandlerType);
                                var integrationCommand = JsonSerializer.Deserialize(message, commandType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                                var concreteType = typeof(IIntegrationCommandHandler<>).MakeGenericType(commandType);

                                await Task.Yield();
                                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationCommand });
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ command: {CommandName}", commandName);
            }
        }
    }

    public class CommandBusRabbitMQ<TOwningService> : CommandBusRabbitMQ, ICommandBus<TOwningService>, IDisposable
    where TOwningService : IOwningService
    {
        public CommandBusRabbitMQ(CommandBusConfiguration<TOwningService> commandBusConfiguration,
            ILogger<DefaultRabbitMQPersistentConnection> clogger,
            ILogger<CommandBusRabbitMQ<TOwningService>> logger,
            ILifetimeScope autofac,
            ICommandBusSubscriptionsManager<TOwningService> subsManager)
            : base(commandBusConfiguration, clogger, logger, autofac, subsManager)
        {
        }
    }
}
