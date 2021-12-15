using MediatR;
using ProductFocus.Domain;
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
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public RemoveOwnerFromWorkItemDomainEventhandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(RemoveOwnerFromWorkItemDomainEvent removeOwnerToWorkItemDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(removeOwnerToWorkItemDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(RemoveOwnerFromWorkItemDomainEvent), JsonSerializer.Serialize(new { FeatureId = removeOwnerToWorkItemDomainEvent.Feature.Id, Title = removeOwnerToWorkItemDomainEvent.Feature.Title, OwnerName = removeOwnerToWorkItemDomainEvent.OwnerName, OwnerEmail = removeOwnerToWorkItemDomainEvent.OwnerEmail }), removeOwnerToWorkItemDomainEvent.Feature.Module?.Id, removeOwnerToWorkItemDomainEvent.Feature.Module?.Name, removeOwnerToWorkItemDomainEvent.EventTriggeredById, user.Name, removeOwnerToWorkItemDomainEvent.ProductId, removeOwnerToWorkItemDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
