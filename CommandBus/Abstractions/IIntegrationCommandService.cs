using CommandBus.Commands;

namespace CommandBus.Abstractions
{
    public interface IIncomingIntegrationCommandService
    {
        Task PublishThroughCommandBusAsync(IntegrationCommand command);
    }
}
