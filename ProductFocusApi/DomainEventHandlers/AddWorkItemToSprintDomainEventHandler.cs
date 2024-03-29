﻿using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class AddWorkItemToSprintDomainEventHandler : INotificationHandler<AddWorkItemToSprintDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public AddWorkItemToSprintDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(AddWorkItemToSprintDomainEvent addWorkItemToSprintDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(addWorkItemToSprintDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(AddWorkItemToSprintDomainEvent), JsonSerializer.Serialize(new { addWorkItemToSprintDomainEvent.Feature.Id, addWorkItemToSprintDomainEvent.Feature.Title, addWorkItemToSprintDomainEvent.PreviousSprint, addWorkItemToSprintDomainEvent.CurrentSprint }), addWorkItemToSprintDomainEvent.Feature.ModuleId, addWorkItemToSprintDomainEvent.Feature.Module?.Name, addWorkItemToSprintDomainEvent.EventTriggeredById, user.Name, addWorkItemToSprintDomainEvent.ProductId, addWorkItemToSprintDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
