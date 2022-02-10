using MediatR;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading;
using System.Text.Json;
using ProductFocus.Domain.Common;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemBlockedDomainEventHandler
         : INotificationHandler<WorkItemBlockedDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public WorkItemBlockedDomainEventHandler(
            IDomainEventLogRepository domainEventLogRepository, 
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
            )
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }


        public System.Threading.Tasks.Task Handle(WorkItemBlockedDomainEvent workItemMarkedAsBlockedDomainEvent, CancellationToken cancellationToken)
        {
            // Log the event in log table 

            // TODO : workItemMarkedAsBlockedDomainEvent.EventTriggeredBy could not be sent as its type is long, but the CreatedBy field in AggregateRoot is a string, which needs
            // to be converted to long 
            User user = _userRepository.GetById(workItemMarkedAsBlockedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemBlockedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemMarkedAsBlockedDomainEvent.Feature.Id, workItemMarkedAsBlockedDomainEvent.Feature.Title }), workItemMarkedAsBlockedDomainEvent.Feature.ModuleId, workItemMarkedAsBlockedDomainEvent.Feature.Module?.Name, workItemMarkedAsBlockedDomainEvent.EventTriggeredById, user.Name, workItemMarkedAsBlockedDomainEvent.ProductId, workItemMarkedAsBlockedDomainEvent.Feature.Id);

            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
