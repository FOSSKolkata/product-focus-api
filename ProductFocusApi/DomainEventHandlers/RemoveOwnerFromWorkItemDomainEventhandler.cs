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
    public class RemoveOwnerFromWorkItemDomainEventhandler : INotificationHandler<RemoveOwnerFromWorkItemDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public RemoveOwnerFromWorkItemDomainEventhandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(RemoveOwnerFromWorkItemDomainEvent removeOwnerToWorkItemDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(removeOwnerToWorkItemDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(RemoveOwnerFromWorkItemDomainEvent), JsonSerializer.Serialize(new { removeOwnerToWorkItemDomainEvent.Feature.Id, removeOwnerToWorkItemDomainEvent.Feature.Title, removeOwnerToWorkItemDomainEvent.OwnerName, removeOwnerToWorkItemDomainEvent.OwnerEmail }), removeOwnerToWorkItemDomainEvent.Feature.Module?.Id, removeOwnerToWorkItemDomainEvent.Feature.Module?.Name, removeOwnerToWorkItemDomainEvent.EventTriggeredById, user.Name, removeOwnerToWorkItemDomainEvent.ProductId, removeOwnerToWorkItemDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
