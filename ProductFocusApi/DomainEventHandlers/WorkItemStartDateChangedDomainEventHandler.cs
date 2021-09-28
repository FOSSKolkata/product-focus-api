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
    public class WorkItemStartDateChangedDomainEventHandler : INotificationHandler<WorkItemStartDateChangedDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public WorkItemStartDateChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(WorkItemStartDateChangedDomainEvent workItemStartDateChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemStartDateChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new WorkItemDomainEventLog(nameof(WorkItemStartDateChangedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemStartDateChangedDomainEvent.Feature.Id, Title = workItemStartDateChangedDomainEvent.Feature.Title, PreviousStartDate = workItemStartDateChangedDomainEvent.PreviousStartDate, CurrentStartDate = workItemStartDateChangedDomainEvent.CurrentStartDate }), workItemStartDateChangedDomainEvent.Feature.ModuleId, workItemStartDateChangedDomainEvent.Feature.Module.Name, workItemStartDateChangedDomainEvent.EventTriggeredById, user.Name, workItemStartDateChangedDomainEvent.ProductId, workItemStartDateChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
