using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class DeleteBusinessRequirementCommand : ICommand
    {
        public virtual long Id { get; private set; }
        public virtual string UserId { get; private set; }
        public DeleteBusinessRequirementCommand(long id, string userId)
        {
            Id = id;
            UserId = userId;
        }
        public sealed class DeleteBusinessRequirementCommandHandler : ICommandHandler<DeleteBusinessRequirementCommand>
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

            public async Task<Result> Handle(DeleteBusinessRequirementCommand command)
            {
                try
                {
                    BusinessRequirement businessRequirement = await _businessRequirementRepository.GetById(command.Id);
                    businessRequirement.Delete(command.UserId);
                    await _unitOfWork.CompleteAsync();
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
