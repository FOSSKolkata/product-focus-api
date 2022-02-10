using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class UpdateBusinessRequirementCommand: ICommand
    {
        public long Id { get; private set; }
        public string Title { get; private set; }
        public DateTime ReceivedOn { get; private set; }
        public IList<long> TagIds { get; private set; }
        public BusinessRequirementSourceEnum SourceEnum { get; private set; }
        public string SourceAdditionalInformation { get; private set; }
        public string Description { get; private set; }

        public UpdateBusinessRequirementCommand(long id,
            string title, DateTime receivedOn, IList<long> tagIds,
            BusinessRequirementSourceEnum sourceEnum,
            string sourceAdditionalInformation,
            string description)
        {
            Id = id;
            Title = title;
            ReceivedOn = receivedOn;
            TagIds = tagIds;
            SourceEnum = sourceEnum;
            SourceAdditionalInformation = sourceAdditionalInformation;
            Description = description;
        }
        internal sealed class UpdateBusinessRequirementCommandHandler : ICommandHandler<UpdateBusinessRequirementCommand>
        {
            private readonly IBusinessRequirementRepository _businessRequirementRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateBusinessRequirementCommandHandler(IBusinessRequirementRepository businessRequirementRepository,
                IUnitOfWork unitOfWork)
            {
                _businessRequirementRepository = businessRequirementRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(UpdateBusinessRequirementCommand command)
            {
                try
                {
                    BusinessRequirement businessRequirement = await _businessRequirementRepository.GetById(command.Id);
                    businessRequirement.UpdateTitle(command.Title);
                    businessRequirement.UpdateReceivedOn(command.ReceivedOn);
                    businessRequirement.UpdateSourceEnum(command.SourceEnum);
                    businessRequirement.UpdateSourceInformation(command.SourceAdditionalInformation);
                    businessRequirement.UpdateDescription(command.Description);

                    await _unitOfWork.CompleteAsync();
                    return Result.Success();
                }catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
            }
        }
    }
}
