using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Events;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Text.Json;
using System.Threading;

namespace ProductFocusApi.DomainEventHandlers
{
    public class WorkItemStartDateChangedDomainEventHandler : INotificationHandler<WorkItemStartDateChangedDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public WorkItemStartDateChangedDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(WorkItemStartDateChangedDomainEvent workItemStartDateChangedDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(workItemStartDateChangedDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(WorkItemStartDateChangedDomainEvent), JsonSerializer.Serialize(new { workItemStartDateChangedDomainEvent.Feature.Id, workItemStartDateChangedDomainEvent.Feature.Title, workItemStartDateChangedDomainEvent.PreviousStartDate, workItemStartDateChangedDomainEvent.CurrentStartDate }), workItemStartDateChangedDomainEvent.Feature.ModuleId, workItemStartDateChangedDomainEvent.Feature.Module?.Name, workItemStartDateChangedDomainEvent.EventTriggeredById, user.Name, workItemStartDateChangedDomainEvent.ProductId, workItemStartDateChangedDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
