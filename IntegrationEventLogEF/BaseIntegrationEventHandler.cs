using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF.Services;

namespace IntegrationEventLogEF
{
    public abstract class BaseIntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>
            where TIntegrationEvent : IntegrationEvent
    {
        IIncomingIntegrationEventLogService _integrationEventLogService;
        public BaseIntegrationEventHandler(IIncomingIntegrationEventLogService integrationEventLogService)
        {
            _integrationEventLogService = integrationEventLogService;
        }


        public abstract Task Handle(TIntegrationEvent @event);

        public async Task<bool> Preprocess(TIntegrationEvent @event)
        {
            IncomingIntegrationEventLogEntry integrationEventLog = await _integrationEventLogService.RetrieveEventLogAsync(@event.Id);

            if (integrationEventLog == null)
            {
                await _integrationEventLogService.SaveAndMarkEventAsInProgressAsync(@event);
            }
            else
            {
                if (integrationEventLog.State == IncomingEventStateEnum.ProcessingInProgress)
                    return false;
            }

            return true;
        }

        public async Task PostprocessOnSuccess(TIntegrationEvent @event)
        {
            await _integrationEventLogService.MarkEventAsProcessedAsync(@event.Id);
        }

        public async Task PostprocessOnFailure(TIntegrationEvent @event, Exception ex)
        {
            await _integrationEventLogService.MarkEventAsFailedAsync(@event.Id, ex);
        }
    }
}
