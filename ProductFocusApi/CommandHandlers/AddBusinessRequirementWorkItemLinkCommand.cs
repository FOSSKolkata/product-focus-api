using CSharpFunctionalExtensions;
using MediatR;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class AddBusinessRequirementWorkItemLinkCommand : IRequest<Result>
    {
        public long WorkItemId { get; private set; }
        public long BusinessRequirementId { get; private set; }
        public AddBusinessRequirementWorkItemLinkCommand(long workItemId, long businessRequirementId)
        {
            WorkItemId = workItemId;
            BusinessRequirementId = businessRequirementId;
        }
        internal sealed class AddBusinessRequirementWorkItemLinkCommandHandler : IRequestHandler<AddBusinessRequirementWorkItemLinkCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IBusinessRequirementWorkItemLinkRepository _businessRequirementWorkItemLinkRepository;
            public AddBusinessRequirementWorkItemLinkCommandHandler(IUnitOfWork unitOfWork, IBusinessRequirementWorkItemLinkRepository businessRequirementWorkItemLinkRepository)
            {
                _unitOfWork = unitOfWork;
                _businessRequirementWorkItemLinkRepository = businessRequirementWorkItemLinkRepository;
            }

            public async Task<Result> Handle(AddBusinessRequirementWorkItemLinkCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    BusinessRequirementToWorkItemLink businessRequirementToWorkItem =
                        BusinessRequirementToWorkItemLink.CreateInstance(request.WorkItemId, request.BusinessRequirementId);
                    _businessRequirementWorkItemLinkRepository.Add(businessRequirementToWorkItem);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }catch(Exception)
                {
                    throw;
                }
                return Result.Success();
            }
        }
    }
}
