using MediatR;
using ProductFocus.Domain;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemEndDateChangedDomainEventHandler : INotificationHandler<WorkItemEndDateChangedDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public WorkItemEndDateChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async System.Threading.Tasks.Task Handle(WorkItemEndDateChangedDomainEvent workItemEndDateChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemEndDateChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemEndDateChangedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemEndDateChangedDomainEvent.Feature.Id, Title = workItemEndDateChangedDomainEvent.Feature.Title, PreviousEndDate = workItemEndDateChangedDomainEvent.PreviousEndDate, CurrentEndDate = workItemEndDateChangedDomainEvent.CurrentEndDate }), workItemEndDateChangedDomainEvent.Feature.ModuleId, workItemEndDateChangedDomainEvent.Feature.Module?.Name, workItemEndDateChangedDomainEvent.EventTriggeredById, user.Name, workItemEndDateChangedDomainEvent.ProductId, workItemEndDateChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
