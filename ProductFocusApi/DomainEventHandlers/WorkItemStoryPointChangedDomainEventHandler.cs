using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemStoryPointChangedDomainEventHandler : INotificationHandler<WorkItemStoryPointChangedDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public WorkItemStoryPointChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(WorkItemStoryPointChangedDomainEvent workItemStoryPointChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemStoryPointChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemStoryPointChangedDomainEvent), JsonSerializer.Serialize(new { workItemStoryPointChangedDomainEvent.Feature.Id, workItemStoryPointChangedDomainEvent.Feature.Title, workItemStoryPointChangedDomainEvent.PreviousStoryPoint, workItemStoryPointChangedDomainEvent.CurrentStoryPoint }), workItemStoryPointChangedDomainEvent.Feature.ModuleId, workItemStoryPointChangedDomainEvent.Feature.Module?.Name, workItemStoryPointChangedDomainEvent.EventTriggeredById, user.Name, workItemStoryPointChangedDomainEvent.ProductId, workItemStoryPointChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
