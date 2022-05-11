using CommandBus.Abstractions;
using CommandBus.Commands;

namespace CommandBus
{
    public partial class InMemoryCommandBusSubscriptionsManager : ICommandBusSubscriptionsManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _commandTypes;

        public event EventHandler<string> OnCommandRemoved;

        public InMemoryCommandBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<SubscriptionInfo>>();
            _commandTypes = new List<Type>();
        }

        public bool IsEmpty => !_handlers.Keys.Any();
        public void Clear() => _handlers.Clear();

        public void AddDynamicSubscription<TH>(string commandName)
            where TH : IDynamicIntegrationCommandHandler
        {
            DoAddSubscription(typeof(TH), isDynamic: true);
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationCommand
            where TH : IIntegrationCommandHandler<T>
        {
            var commandName = GetCommandKey<T>();

            DoAddSubscription(typeof(TH), isDynamic: false, typeof(T));

            if (!_commandTypes.Contains(typeof(T)))
            {
                _commandTypes.Add(typeof(T));
            }
        }

        private void DoAddSubscription(Type handlerType, bool isDynamic, Type commandType = null)
        {
            if (!HasSubscriptionsForCommand(commandType.Name))
            {
                _handlers.Add(commandType.Name, new List<SubscriptionInfo>());
            }

            if (_handlers[commandType.Name].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{commandType.Name}'", nameof(handlerType));
            }

            if (isDynamic)
            {
                _handlers[commandType.Name].Add(SubscriptionInfo.Dynamic(handlerType));
            }
            else
            {
                _handlers[commandType.Name].Add(SubscriptionInfo.Typed(handlerType, commandType));
            }
        }


        public void RemoveDynamicSubscription<TH>(string commandName)
            where TH : IDynamicIntegrationCommandHandler
        {
            var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(commandName);
            DoRemoveHandler(commandName, handlerToRemove);
        }


        public void RemoveSubscription<T, TH>()
            where TH : IIntegrationCommandHandler<T>
            where T : IntegrationCommand
        {
            var handlerToRemove = FindSubscriptionToRemove<T, TH>();
            var commandName = GetCommandKey<T>();
            DoRemoveHandler(commandName, handlerToRemove);
        }


        private void DoRemoveHandler(string commandName, SubscriptionInfo subsToRemove)
        {
            if (subsToRemove != null)
            {
                _handlers[commandName].Remove(subsToRemove);
                if (!_handlers[commandName].Any())
                {
                    _handlers.Remove(commandName);
                    var commandType = _commandTypes.SingleOrDefault(e => e.Name == commandName);
                    if (commandType != null)
                    {
                        _commandTypes.Remove(commandType);
                    }
                    RaiseOnCommandRemoved(commandName);
                }

            }
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForCommand<T>() where T : IntegrationCommand
        {
            var key = GetCommandKey<T>();
            return GetHandlersForCommand(key);
        }
        public IEnumerable<SubscriptionInfo> GetHandlersForCommand(string commandName) => _handlers[commandName];

        private void RaiseOnCommandRemoved(string commandName)
        {
            var handler = OnCommandRemoved;
            handler?.Invoke(this, commandName);
        }


        private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string commandName)
            where TH : IDynamicIntegrationCommandHandler
        {
            return DoFindSubscriptionToRemove(commandName, typeof(TH));
        }


        private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
             where T : IntegrationCommand
             where TH : IIntegrationCommandHandler<T>
        {
            var commandName = GetCommandKey<T>();
            return DoFindSubscriptionToRemove(commandName, typeof(TH));
        }

        private SubscriptionInfo DoFindSubscriptionToRemove(string commandName, Type handlerType)
        {
            if (!HasSubscriptionsForCommand(commandName))
            {
                return null;
            }

            return _handlers[commandName].SingleOrDefault(s => s.HandlerType == handlerType);

        }

        public bool HasSubscriptionsForCommand<T>() where T : IntegrationCommand
        {
            var key = GetCommandKey<T>();
            return HasSubscriptionsForCommand(key);
        }
        public bool HasSubscriptionsForCommand(string commandName) => _handlers.ContainsKey(commandName);

        public Type GetCommandTypeByName(string commandName, Type handlerType)
        {
            List<SubscriptionInfo> subscriptionInfos;
            if (_handlers.TryGetValue(commandName, out subscriptionInfos))
            {
                var subscriptionInfo = subscriptionInfos.Where(x => x.HandlerType == handlerType).SingleOrDefault();

                if (subscriptionInfo != null)
                    return subscriptionInfo.CommandType;
            }


            return null;
        }

        public string GetCommandKey<T>()
        {
            return typeof(T).Name;
        }
    }


    public class InMemoryCommandBusSubscriptionsManager<TOwningService> : InMemoryCommandBusSubscriptionsManager, ICommandBusSubscriptionsManager<TOwningService>
        where TOwningService : IOwningService
    { }
}
