using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemEndDateChangedDomainEventHandler : INotificationHandler<WorkItemEndDateChangedDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public WorkItemEndDateChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async System.Threading.Tasks.Task Handle(WorkItemEndDateChangedDomainEvent workItemEndDateChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemEndDateChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemEndDateChangedDomainEvent), JsonSerializer.Serialize(new { workItemEndDateChangedDomainEvent.Feature.Id, workItemEndDateChangedDomainEvent.Feature.Title, workItemEndDateChangedDomainEvent.PreviousEndDate, workItemEndDateChangedDomainEvent.CurrentEndDate }), workItemEndDateChangedDomainEvent.Feature.ModuleId, workItemEndDateChangedDomainEvent.Feature.Module?.Name, workItemEndDateChangedDomainEvent.EventTriggeredById, user.Name, workItemEndDateChangedDomainEvent.ProductId, workItemEndDateChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
