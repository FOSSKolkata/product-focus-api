using MediatR;
using ProductFocus.Domain;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemStoryPointChangedDomainEventHandler : INotificationHandler<WorkItemStoryPointChangedDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public WorkItemStoryPointChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(WorkItemStoryPointChangedDomainEvent workItemStoryPointChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemStoryPointChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemStoryPointChangedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemStoryPointChangedDomainEvent.Feature.Id, Title = workItemStoryPointChangedDomainEvent.Feature.Title, PreviousStoryPoint = workItemStoryPointChangedDomainEvent.PreviousStoryPoint, CurrentStoryPoint = workItemStoryPointChangedDomainEvent.CurrentStoryPoint }), workItemStoryPointChangedDomainEvent.Feature.ModuleId, workItemStoryPointChangedDomainEvent.Feature.Module?.Name, workItemStoryPointChangedDomainEvent.EventTriggeredById, user.Name, workItemStoryPointChangedDomainEvent.ProductId, workItemStoryPointChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
