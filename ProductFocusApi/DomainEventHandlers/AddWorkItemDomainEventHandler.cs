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
    public class AddWorkItemDomainEventHandler : INotificationHandler<AddWorkItemDomainEvent>
    {
        private readonly IDomainEventLogRepository _domainEventLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public AddWorkItemDomainEventHandler(IDomainEventLogRepository domainEventLogRepository, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _domainEventLogRepository = domainEventLogRepository ?? throw new ArgumentNullException(nameof(domainEventLogRepository));
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }
        public async System.Threading.Tasks.Task Handle(AddWorkItemDomainEvent addWorkItemDomainEvent, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetById(addWorkItemDomainEvent.EventTriggeredById);
            WorkItemDomainEventLog workItemDomainEventLog = new(nameof(AddWorkItemDomainEvent), JsonSerializer.Serialize(new { addWorkItemDomainEvent.Feature.Id, addWorkItemDomainEvent.Feature.Title }), addWorkItemDomainEvent.Feature.ModuleId, addWorkItemDomainEvent.Feature.Module?.Name, addWorkItemDomainEvent.EventTriggeredById, user.Name, addWorkItemDomainEvent.ProductId, addWorkItemDomainEvent.Feature.Id);
            _domainEventLogRepository.AddDomainEventLog(workItemDomainEventLog);

            await _unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
