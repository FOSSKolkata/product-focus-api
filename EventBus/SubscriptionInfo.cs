namespace EventBus
{
    public partial class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
    {
        public class SubscriptionInfo
        {
            public bool IsDynamic { get; }
            public Type HandlerType { get; }

            public Type EventType { get; }
            private SubscriptionInfo(bool isDynamic, Type handlerType, Type eventType = null)
            {
                IsDynamic = isDynamic;
                HandlerType = handlerType;
                EventType = eventType;
            }

            public static SubscriptionInfo Dynamic(Type handlerType)
            {
                return new SubscriptionInfo(true, handlerType);
            }
            public static SubscriptionInfo Typed(Type handlerType, Type eventType)
            {
                return new SubscriptionInfo(false, handlerType, eventType);
            }
        }
    }
}
