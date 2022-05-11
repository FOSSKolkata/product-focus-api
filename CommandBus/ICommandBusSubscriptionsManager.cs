using CommandBus.Abstractions;
using CommandBus.Commands;

namespace CommandBus
{
    public interface ICommandBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnCommandRemoved;
        void AddDynamicSubscription<TH>(string CommandName)
           where TH : IDynamicIntegrationCommandHandler;

        void AddSubscription<T, TH>()
           where T : IntegrationCommand
           where TH : IIntegrationCommandHandler<T>;

        void RemoveSubscription<T, TH>()
             where TH : IIntegrationCommandHandler<T>
             where T : IntegrationCommand;
        void RemoveDynamicSubscription<TH>(string CommandName)
            where TH : IDynamicIntegrationCommandHandler;

        bool HasSubscriptionsForCommand<T>() where T : IntegrationCommand;
        bool HasSubscriptionsForCommand(string commandName);
        Type GetCommandTypeByName(string commandName, Type handlerType);
        void Clear();
        IEnumerable<SubscriptionInfo> GetHandlersForCommand<T>() where T : IntegrationCommand;
        IEnumerable<SubscriptionInfo> GetHandlersForCommand(string commandName);
        string GetCommandKey<T>();
    }

    public interface ICommandBusSubscriptionsManager<TOwningService> : ICommandBusSubscriptionsManager
        where TOwningService : IOwningService
    { }
}