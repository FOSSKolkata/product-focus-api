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
    public class AddOwnerToWorkItemDomainEventHandler : INotificationHandler<AddOwnerToWorkItemDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public AddOwnerToWorkItemDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(AddOwnerToWorkItemDomainEvent addOwnerToWorkItemDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(addOwnerToWorkItemDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new WorkItemDomainEventLog(nameof(AddOwnerToWorkItemDomainEvent), JsonSerializer.Serialize(new { FeatureId = addOwnerToWorkItemDomainEvent.Feature.Id, Title = addOwnerToWorkItemDomainEvent.Feature.Title, OwnerName = addOwnerToWorkItemDomainEvent.OwnerName, OwnerEmail = addOwnerToWorkItemDomainEvent.OwnerEmail }), addOwnerToWorkItemDomainEvent.Feature.ModuleId, addOwnerToWorkItemDomainEvent.Feature.Module?.Name, addOwnerToWorkItemDomainEvent.EventTriggeredById, user.Name, addOwnerToWorkItemDomainEvent.ProductId, addOwnerToWorkItemDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
