using CommandBus.Commands;

namespace CommandBus.Abstractions
{
    public interface IIntegrationCommandHandler<in TIntegrationCommand> : IIntegrationCommandHandler
        where TIntegrationCommand : IntegrationCommand
    {
        Task Handle(TIntegrationCommand command);
    }

    public interface IIntegrationCommandHandler
    {
    }
}
