﻿using BusinessRequirements.Domain.Common;
using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Repositories;
using CSharpFunctionalExtensions;
using MediatR;

namespace BusinessRequirements.CommandHandlers
{
    public class DeleteBusinessRequirementCommand : IRequest<Result>
    {
        public virtual long Id { get; private set; }
        public virtual string UserId { get; private set; }
        public DeleteBusinessRequirementCommand(long id, string userId)
        {
            Id = id;
            UserId = userId;
        }
        public sealed class DeleteBusinessRequirementCommandHandler : IRequestHandler<DeleteBusinessRequirementCommand, Result>
        {
            private readonly IBusinessRequirementRepository _businessRequirementRepository;
            private readonly IUnitOfWork _unitOfWork;
            public DeleteBusinessRequirementCommandHandler(IBusinessRequirementRepository businessRequirementRepository,
                IUnitOfWork unitOfWork
                )
            {
                _businessRequirementRepository = businessRequirementRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result> Handle(DeleteBusinessRequirementCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    BusinessRequirement businessRequirement = await _businessRequirementRepository.GetById(request.Id);
                    businessRequirement.Delete(request.UserId);
                    await _unitOfWork.CompleteAsync(cancellationToken);
                    return Result.Success();
                }
                catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
