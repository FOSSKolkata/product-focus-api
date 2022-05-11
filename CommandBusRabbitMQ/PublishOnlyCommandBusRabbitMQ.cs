using Autofac;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CommandBus.Abstractions;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using Polly;
using RabbitMQ.Client;
using CommandBus;
using CommandBus.Commands;

namespace CommandBusRabbitMQ
{

    public class PublishOnlyCommandBusRabbitMQ<TOtherService> : IPublishOnlyCommandBus<TOtherService>
       where TOtherService : IOtherService

    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<PublishOnlyCommandBusRabbitMQ<TOtherService>> _logger;
        private readonly int _retryCount;
        private readonly string _brokerName;

        public PublishOnlyCommandBusRabbitMQ(CommandBusConfiguration commandBusConfiguration,
            TOtherService otherService,
            ILogger<DefaultRabbitMQPersistentConnection> clogger,
            ILogger<PublishOnlyCommandBusRabbitMQ<TOtherService>> logger)
        {
            OtherConnection otherConnection = commandBusConfiguration.OtherConnections.Where(x => x.Service == otherService.Name).Single();

            var factory = new ConnectionFactory()
            {
                HostName = otherConnection.ConnectionString, // configuration["EventBusConnection"],
                DispatchConsumersAsync = true
            };

            //if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
            //{
            //    factory.UserName = configuration["EventBusUserName"];
            //}

            //if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
            //{
            //    factory.Password = configuration["EventBusPassword"];
            //}

            //var retryCount = 5;
            //if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
            //{
            //    retryCount = int.Parse(configuration["EventBusRetryCount"]);
            //}

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _retryCount = 5;
            _brokerName = otherConnection.Service + "_exchange";
            _persistentConnection = new DefaultRabbitMQPersistentConnection(factory, clogger, _retryCount);
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
    }

    public class PublishOnlyCommandBusRabbitMQ<TOwningService, TOtherService> :
        PublishOnlyCommandBusRabbitMQ<TOtherService>,
        IPublishOnlyCommandBus<TOwningService, TOtherService>
    where TOwningService : IOwningService
    where TOtherService : IOtherService

    {
        public PublishOnlyCommandBusRabbitMQ(
            CommandBusConfiguration<TOwningService> commandBusConfiguration,
            TOtherService otherService,
            ILogger<DefaultRabbitMQPersistentConnection> clogger,
            ILogger<PublishOnlyCommandBusRabbitMQ<TOwningService, TOtherService>> logger)
            : base(commandBusConfiguration, otherService, clogger, logger)
        { }
    }
}
