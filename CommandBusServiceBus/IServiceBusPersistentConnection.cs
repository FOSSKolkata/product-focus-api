namespace CommandBusServiceBus
{
    using Microsoft.Azure.ServiceBus;
    using System;

    public interface IServiceBusPersistentConnection : IDisposable
    {
        IQueueClient QueueClient { get; }
    }
}