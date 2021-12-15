using MediatR;
using ProductFocus.Domain;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemTitleChangedDomainEventHandler : INotificationHandler<WorkItemTitleChangedDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public WorkItemTitleChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(WorkItemTitleChangedDomainEvent workItemTitleChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemTitleChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemTitleChangedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemTitleChangedDomainEvent.Feature.Id, Title = workItemTitleChangedDomainEvent.Feature.Title, PreviousTitle = workItemTitleChangedDomainEvent.PreviousTitle, CurrentTitle = workItemTitleChangedDomainEvent.CurrentTitle }), workItemTitleChangedDomainEvent.Feature.ModuleId, workItemTitleChangedDomainEvent.Feature.Module?.Name, workItemTitleChangedDomainEvent.EventTriggeredById, user.Name, workItemTitleChangedDomainEvent.ProductId, workItemTitleChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
