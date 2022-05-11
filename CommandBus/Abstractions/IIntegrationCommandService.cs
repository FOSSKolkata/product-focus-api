using CommandBus.Commands;

namespace CommandBus.Abstractions
{
    public interface IIntegrationCommandService
    {
        Task PublishThroughCommandBusAsync(IntegrationCommand command);
    }
}
