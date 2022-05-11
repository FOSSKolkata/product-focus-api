namespace EventBus.Abstractions
{
    public class EventBusConfiguration
    {
        public string OwningService { get; set; }
        public string ConnectionString { get; set; }
        public string SubscriptionName { get; set; }
    }

    public class EventBusConfiguration<TOwningService> : EventBusConfiguration
        where TOwningService : IEventBusOwningService
    {
        public static EventBusConfiguration<TOwningService> From(EventBusConfiguration obj)
        {
            return new EventBusConfiguration<TOwningService>()
            {
                OwningService = obj.OwningService,
                ConnectionString = obj.ConnectionString,
                SubscriptionName = obj.SubscriptionName
            };
        }
    }
}
