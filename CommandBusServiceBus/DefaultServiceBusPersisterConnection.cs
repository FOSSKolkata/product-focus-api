using Microsoft.Azure.ServiceBus;

namespace CommandBusServiceBus
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersistentConnection
    {
        private readonly ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;
        private IQueueClient _queueClient;

        bool _disposed;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder)
        {
            _serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            _queueClient = new QueueClient(_serviceBusConnectionStringBuilder, retryPolicy: RetryPolicy.Default);
        }

        public IQueueClient QueueClient
        {
            get
            {
                if (_queueClient.IsClosedOrClosing)
                {
                    _queueClient = new QueueClient(_serviceBusConnectionStringBuilder, retryPolicy: RetryPolicy.Default);
                }
                return _queueClient;
            }
        }
        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => _serviceBusConnectionStringBuilder;
        public IQueueClient CreateModel()
        {
            if (_queueClient.IsClosedOrClosing)
            {
                _queueClient = new QueueClient(_serviceBusConnectionStringBuilder, retryPolicy: RetryPolicy.Default);
            }

            return _queueClient;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
        }
    }
}
