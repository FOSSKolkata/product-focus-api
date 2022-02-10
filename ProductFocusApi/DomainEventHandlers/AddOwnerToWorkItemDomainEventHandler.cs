using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class AddOwnerToWorkItemDomainEventHandler : INotificationHandler<AddOwnerToWorkItemDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public AddOwnerToWorkItemDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(AddOwnerToWorkItemDomainEvent addOwnerToWorkItemDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(addOwnerToWorkItemDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(AddOwnerToWorkItemDomainEvent), JsonSerializer.Serialize(new {addOwnerToWorkItemDomainEvent.Feature.Id, addOwnerToWorkItemDomainEvent.Feature.Title, addOwnerToWorkItemDomainEvent.OwnerName, addOwnerToWorkItemDomainEvent.OwnerEmail }), addOwnerToWorkItemDomainEvent.Feature.ModuleId, addOwnerToWorkItemDomainEvent.Feature.Module?.Name, addOwnerToWorkItemDomainEvent.EventTriggeredById, user.Name, addOwnerToWorkItemDomainEvent.ProductId, addOwnerToWorkItemDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
