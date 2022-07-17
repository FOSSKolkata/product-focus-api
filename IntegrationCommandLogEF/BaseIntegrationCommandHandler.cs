using CommandBus.Abstractions;
using CommandBus.Commands;
using IntegrationCommandLogEF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationCommandLogEF
{
    public abstract  class BaseIntegrationCommandHandler<TIntegrationCommand> : IIntegrationCommandHandler<TIntegrationCommand>
        where TIntegrationCommand : IntegrationCommand
    {
        IIncomingIntegrationCommandLogService _integrationCommandLogService;
        public BaseIntegrationCommandHandler(IIncomingIntegrationCommandLogService integrationCommandLogService)
        {
            _integrationCommandLogService = integrationCommandLogService;
        }


        public abstract Task Handle(TIntegrationCommand command);

        public async Task<bool> Preprocess(TIntegrationCommand command)
        {
            IncomingIntegrationCommandLogEntry integrationCommandLog = await _integrationCommandLogService.RetrieveCommandLogAsync(command.Id);

            if (integrationCommandLog == null)
            {
                await _integrationCommandLogService.SaveAndMarkCommandAsInProgressAsync(command);
            }
            else
            {
                if (integrationCommandLog.State == IncomingCommandStateEnum.ProcessingInProgress)
                    return false;
            }

            return true;
        }

        public async Task PostprocessOnSuccess(TIntegrationCommand command)
        {
            await _integrationCommandLogService.MarkCommandAsProcessedAsync(command.Id);
        }

        public async Task PostprocessOnFailure(TIntegrationCommand command, Exception ex)
        {
            await _integrationCommandLogService.MarkCommandAsFailedAsync(command.Id, ex);
        }
    }
}
