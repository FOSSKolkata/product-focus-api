using CommandBus.Commands;

namespace CommandBus.Abstractions
{
    public interface ICommandBus<TOwningService> : ICommandBus
        where TOwningService : IOwningService
    {

    }

    public interface ICommandBus
    {
        Task PublishAsync(IntegrationCommand command);
        void Subscribe<T, TH>()
           where T : IntegrationCommand
           where TH : IIntegrationCommandHandler<T>;
        void Unsubscribe<T, TH>()
           where TH : IIntegrationCommandHandler<T>
           where T : IntegrationCommand;
        void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationCommandHandler;
        void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationCommandHandler;
    }
}
