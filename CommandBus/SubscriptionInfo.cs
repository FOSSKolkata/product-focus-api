namespace CommandBus
{
    public class SubscriptionInfo
    {
        public bool IsDynamic { get; }
        public Type HandlerType { get; }

        public Type CommandType { get; }
        private SubscriptionInfo(bool isDynamic, Type handlerType, Type commandType = null)
        {
            IsDynamic = isDynamic;
            HandlerType = handlerType;
            CommandType = commandType;
        }

        public static SubscriptionInfo Dynamic(Type handlerType)
        {
            return new SubscriptionInfo(true, handlerType);
        }
        public static SubscriptionInfo Typed(Type handlerType, Type commandType)
        {
            return new SubscriptionInfo(false, handlerType, commandType);
        }
    }
}
