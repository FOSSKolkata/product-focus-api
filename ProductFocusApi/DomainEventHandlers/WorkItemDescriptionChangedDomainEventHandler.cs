using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemDescriptionChangedDomainEventHandler : INotificationHandler<WorkItemDescriptionChangedDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public WorkItemDescriptionChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async System.Threading.Tasks.Task Handle(WorkItemDescriptionChangedDomainEvent workItemDescriptionChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemDescriptionChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemDescriptionChangedDomainEvent), JsonSerializer.Serialize(new { workItemDescriptionChangedDomainEvent.Feature.Id, workItemDescriptionChangedDomainEvent.Feature.Title, workItemDescriptionChangedDomainEvent.PreviousDescription, workItemDescriptionChangedDomainEvent.CurrentDescription }), workItemDescriptionChangedDomainEvent.Feature.ModuleId, workItemDescriptionChangedDomainEvent.Feature.Module?.Name, workItemDescriptionChangedDomainEvent.EventTriggeredById, user.Name, workItemDescriptionChangedDomainEvent.ProductId, workItemDescriptionChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
