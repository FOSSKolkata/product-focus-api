using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF.Services;

namespace IntegrationEventLogEF
{
    public abstract class BaseIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IntegrationEvent
    {
        IIntegrationEventLogService _integrationEventLogService;
        public BaseIntegrationEventHandler(IIntegrationEventLogService integrationEventLogService)
        {
            _integrationEventLogService = integrationEventLogService;
        }


        public abstract Task Handle(TIntegrationEvent @event);

        public async Task<bool> Preprocess(TIntegrationEvent @event)
        {
            IntegrationEventLogEntry integrationEventLog = await _integrationEventLogService.RetrieveEventLogAsync(@event.Id);

            if (integrationEventLog == null)
            {
                await _integrationEventLogService.SaveAndMarkIncomingEventAsInProgressAsync(@event);
            }
            else
            {
                if (integrationEventLog.State == EventStateEnum.ProcessingInProgress)
                    return false;
            }

            return true;
        }

        public async Task PostprocessOnSuccess(TIntegrationEvent @event)
        {
            await _integrationEventLogService.MarkIncomingEventAsProcessedAsync(@event.Id);
        }

        public async Task PostprocessOnFailure(TIntegrationEvent @event, Exception ex)
        {
            await _integrationEventLogService.MarkIncomingEventAsFailedAsync(@event.Id, ex);
        }
    }
}
