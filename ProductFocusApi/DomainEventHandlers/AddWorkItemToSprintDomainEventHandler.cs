using MediatR;
using ProductFocus.Domain;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.DomainEventHandlers
{
    public class AddWorkItemToSprintDomainEventHandler : INotificationHandler<AddWorkItemToSprintDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public AddWorkItemToSprintDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(AddWorkItemToSprintDomainEvent addWorkItemToSprintDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(addWorkItemToSprintDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new WorkItemDomainEventLog(nameof(AddWorkItemToSprintDomainEvent), JsonSerializer.Serialize(new { FeatureId = addWorkItemToSprintDomainEvent.Feature.Id, Title = addWorkItemToSprintDomainEvent.Feature.Title, PreviousSprint = addWorkItemToSprintDomainEvent.PreviousSprint, CurrentSprint = addWorkItemToSprintDomainEvent.CurrentSprint }), addWorkItemToSprintDomainEvent.Feature.ModuleId, addWorkItemToSprintDomainEvent.Feature.Module.Name, addWorkItemToSprintDomainEvent.EventTriggeredById, user.Name, addWorkItemToSprintDomainEvent.ProductId, addWorkItemToSprintDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
