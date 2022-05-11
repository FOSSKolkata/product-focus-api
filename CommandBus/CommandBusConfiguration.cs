using CommandBus.Abstractions;

namespace CommandBus
{
    public class CommandBusConfiguration<TOwningService> : CommandBusConfiguration
            where TOwningService : IOwningService
    {

    }

    public class CommandBusConfiguration
    {
        public string OwningService { get; set; }
        public string MyConnectionString { get; set; }
        public List<OtherConnection> OtherConnections { get; set; }
    }

    public class OtherConnection
    {
        public string Service { get; set; }
        public string ConnectionString { get; set; }
    }
}
