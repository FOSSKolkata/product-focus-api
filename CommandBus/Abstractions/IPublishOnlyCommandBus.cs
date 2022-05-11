using CommandBus.Commands;

namespace CommandBus.Abstractions
{

    public interface IPublishOnlyCommandBus<TOtherService>
        where TOtherService : IOtherService
    {
        Task PublishAsync(IntegrationCommand command);
    }

    public interface IPublishOnlyCommandBus<TOwningService, TOtherService> : IPublishOnlyCommandBus<TOtherService>
        where TOwningService : IOwningService
        where TOtherService : IOtherService
    {

    }
}
