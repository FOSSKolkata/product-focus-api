using CommandBus.Commands;

namespace IntegrationCommandLogEF.Services
{
    public interface IIncomingIntegrationCommandLogService
    {
        Task<IncomingIntegrationCommandLogEntry> RetrieveCommandLogAsync(Guid commandId);
        Task SaveAndMarkCommandAsInProgressAsync(IntegrationCommand command);
        Task MarkCommandAsProcessedAsync(Guid commandId);
        Task MarkCommandAsFailedAsync(Guid commandId, Exception exception);
    }
}
