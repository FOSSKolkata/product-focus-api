using CommandBus.Commands;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationCommandLogEF.Services
{
    public interface IIntegrationCommandLogService
    {
        Task<IEnumerable<IntegrationCommandLogEntry>> RetrieveCommandLogsPendingToPublishAsync(Guid transactionId);
        Task SaveCommandAsync(IntegrationCommand command, IDbContextTransaction transaction);
        Task MarkCommandAsPublishedAsync(Guid commandId);
        Task MarkCommandAsInProgressAsync(Guid commandId);
        Task MarkCommandAsFailedAsync(Guid commandId);
    }
}
