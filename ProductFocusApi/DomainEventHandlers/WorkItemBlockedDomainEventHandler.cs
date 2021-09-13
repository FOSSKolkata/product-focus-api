using MediatR;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using ProductFocus.Domain;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemBlockedDomainEventHandler
         : INotificationHandler<WorkItemBlockedDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        public WorkItemBlockedDomainEventHandler(
            IDomainEventLogRepository domainEventLogRepository, 
            IUnitOfWork unitOfWork)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
        }


        public async System.Threading.Tasks.Task Handle(WorkItemBlockedDomainEvent workItemMarkedAsBlockedDomainEvent, CancellationToken cancellationToken)
        {
            // Log the event in log table 

            // TODO : workItemMarkedAsBlockedDomainEvent.EventTriggeredBy could not be sent as its type is long, but the CreatedBy field in AggregateRoot is a string, which needs
            // to be converted to long
            WorkItemDomainEventLog workItemDomainEventLog = new WorkItemDomainEventLog(nameof(WorkItemBlockedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemMarkedAsBlockedDomainEvent.Feature.Id, Title = workItemMarkedAsBlockedDomainEvent.Feature.Title }), workItemMarkedAsBlockedDomainEvent.Feature.ModuleId, string.Empty, workItemMarkedAsBlockedDomainEvent.ProductId); 

            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
