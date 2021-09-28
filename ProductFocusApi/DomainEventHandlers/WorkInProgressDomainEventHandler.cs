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
    public class WorkInProgressDomainEventHandler : INotificationHandler<WorkInProgressDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;

        public WorkInProgressDomainEventHandler(
            IDomainEventLogRepository domainEventLogRepository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository
            )
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(WorkInProgressDomainEvent workInProgressDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workInProgressDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new WorkItemDomainEventLog(nameof(WorkInProgressDomainEvent), JsonSerializer.Serialize(new { FeatureId = workInProgressDomainEvent.Feature.Id, Title = workInProgressDomainEvent.Feature.Title, OldWorkProgress = workInProgressDomainEvent.OldWorkPercentage, NewWorkProgress = workInProgressDomainEvent.NewWorkPercentage }), workInProgressDomainEvent.Feature.ModuleId, workInProgressDomainEvent.Feature.Module.Name, workInProgressDomainEvent.EventTriggeredById, user.Name, workInProgressDomainEvent.ProductId, workInProgressDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
