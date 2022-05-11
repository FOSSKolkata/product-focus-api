using EventBus.Events;

namespace EventBus.Abstractions
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
        Task<bool> Preprocess(TIntegrationEvent @event);
        Task PostprocessOnSuccess(TIntegrationEvent @event);
        Task PostprocessOnFailure(TIntegrationEvent @event, Exception exception);
    }

    public interface IIntegrationEventHandler
    {
    }
}
