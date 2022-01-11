using MediatR;
using ProductFocus.Domain;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemDescriptionChangedDomainEventHandler : INotificationHandler<WorkItemDescriptionChangedDomainEvent>
    {
        IDomainEventLogRepository _domainEventLogRepository;
        IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        public WorkItemDescriptionChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async System.Threading.Tasks.Task Handle(WorkItemDescriptionChangedDomainEvent workItemDescriptionChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemDescriptionChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemDescriptionChangedDomainEvent), JsonSerializer.Serialize(new { FeatureId = workItemDescriptionChangedDomainEvent.Feature.Id, Title = workItemDescriptionChangedDomainEvent.Feature.Title, PreviousDescription = workItemDescriptionChangedDomainEvent.PreviousDescription, CurrentDescription = workItemDescriptionChangedDomainEvent.CurrentDescription }), workItemDescriptionChangedDomainEvent.Feature.ModuleId, workItemDescriptionChangedDomainEvent.Feature.Module?.Name, workItemDescriptionChangedDomainEvent.EventTriggeredById, user.Name, workItemDescriptionChangedDomainEvent.ProductId, workItemDescriptionChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
