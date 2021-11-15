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
    public class AddWorkItemDomainEventHandler : INotificationHandler<AddWorkItemDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public AddWorkItemDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(AddWorkItemDomainEvent addWorkItemDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(addWorkItemDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new WorkItemDomainEventLog(nameof(AddWorkItemDomainEvent), JsonSerializer.Serialize(new { FeatureId = addWorkItemDomainEvent.Feature.Id, Title = addWorkItemDomainEvent.Feature.Title }), addWorkItemDomainEvent.Feature.ModuleId, addWorkItemDomainEvent.Feature.Module?.Name, addWorkItemDomainEvent.EventTriggeredById, user.Name, addWorkItemDomainEvent.ProductId, addWorkItemDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
