namespace CommandBus.Abstractions
{
    public interface IDynamicIntegrationCommandHandler
    {
        Task Handle(dynamic commandData);
    }
}
