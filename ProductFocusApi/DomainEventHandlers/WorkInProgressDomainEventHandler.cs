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
    public class WorkInProgressDomainEventHandler : INotificationHandler<WorkInProgressDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

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
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkInProgressDomainEvent), JsonSerializer.Serialize(new { workInProgressDomainEvent.Feature.Id, workInProgressDomainEvent.Feature.Title, workInProgressDomainEvent.OldWorkPercentage, workInProgressDomainEvent.NewWorkPercentage }), workInProgressDomainEvent.Feature.ModuleId, workInProgressDomainEvent.Feature.Module?.Name, workInProgressDomainEvent.EventTriggeredById, user.Name, workInProgressDomainEvent.ProductId, workInProgressDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
