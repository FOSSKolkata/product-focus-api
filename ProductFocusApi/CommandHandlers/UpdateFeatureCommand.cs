using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Dtos;
using ProductFocus.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class UpdateFeatureCommand : ICommand
    {
        public UpdateFeatureDto UpdateFeatureDto { get; }

        public string IdpUserId { get; }
        public UpdateFeatureCommand(UpdateFeatureDto updateFeatureDto, string idpUserId)
        {
            UpdateFeatureDto = updateFeatureDto;
            IdpUserId = idpUserId;
        }

        internal sealed class UpdateFeatureCommandHandler : ICommandHandler<UpdateFeatureCommand>
        {            
            private readonly IFeatureRepository _featureRepository;
            private readonly ISprintRepository _sprintRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;
            private readonly IFeatureOrderRepository _featureOrderingRepository;

            public UpdateFeatureCommandHandler(
                IProductRepository productRepository, IFeatureRepository featureRepository,
                ISprintRepository sprintRepository, IUserRepository userRepository, 
                IUnitOfWork unitOfWork, IEmailService emailService,
                IFeatureOrderRepository featureOrderRepository)
            {                
                _featureRepository = featureRepository;
                _sprintRepository = sprintRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
                _featureOrderingRepository = featureOrderRepository;
            }
            public async Task<Result> Handle(UpdateFeatureCommand command)
            {
                Feature feature = await _featureRepository.GetById(command.UpdateFeatureDto.Id);
                
                if (feature == null)
                    return Result.Failure($"No feature found with Feature Id '{command.UpdateFeatureDto.Id}'");                
                                

                try
                {
                    User updatedByUser = _userRepository.GetByIdpUserId(command.IdpUserId);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Title)
                        feature.UpdateTitle(command.UpdateFeatureDto.Title, updatedByUser.Id, feature.Title);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Description)
                        feature.UpdateDescription(command.UpdateFeatureDto.Description, updatedByUser.Id, feature.Description);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.WorkCompletionPercentage)
                        feature.UpdateWorkCompletionPercentage(command.UpdateFeatureDto.WorkCompletionPercentage, updatedByUser.Id, feature.WorkCompletionPercentage);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Status)
                        feature.UpdateStatus(command.UpdateFeatureDto.Status);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Sprint)
                    {
                        Sprint currentSprint = _sprintRepository.GetByName(command.UpdateFeatureDto.SprintName);

                        if (currentSprint == null)
                            return Result.Failure($"Sprint with name '{command.UpdateFeatureDto.SprintName}' doesn't exist");
                        feature.UpdateSprint(currentSprint, updatedByUser.Id, feature.Sprint);

                        List<FeatureOrdering> featureOrderings = await _featureOrderingRepository.GetByIdAndSprint(feature.Id, feature.Sprint.Id);
                        foreach (var featureOrdering in featureOrderings)
                        {
                            featureOrdering.UpdateSprint(currentSprint.Id);
                        }
                    }

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.StoryPoint)
                        feature.UpdateStoryPoint(command.UpdateFeatureDto.StoryPoint, updatedByUser.Id, feature.StoryPoint);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IsBlocked)
                    {
                        feature.UpdateBlockedStatus(command.UpdateFeatureDto.IsBlocked, updatedByUser.Id);
                    }

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IncludeAssignee)
                    {
                        User userDetails = _userRepository.GetByEmail(command.UpdateFeatureDto.EmailOfAssignee);

                        if (userDetails == null)
                            return Result.Failure($"User with email '{command.UpdateFeatureDto.EmailOfAssignee}' doesn't exist");

                        feature.IncludeAssignee(userDetails, updatedByUser.Id, command.UpdateFeatureDto.EmailOfAssignee);
                    }

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.ExcludeAssignee)
                    {
                        User userDetails = _userRepository.GetByEmail(command.UpdateFeatureDto.EmailOfAssignee);

                        if (userDetails == null)
                            return Result.Failure($"User with email '{command.UpdateFeatureDto.EmailOfAssignee}' doesn't exist");

                        feature.ExcludeAssignee(userDetails, updatedByUser.Id, command.UpdateFeatureDto.EmailOfAssignee);
                    }

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.AcceptanceCriteria)
                        feature.UpdateAcceptanceCriteria(command.UpdateFeatureDto.AcceptanceCriteria);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.PlannedStartDate)
                        feature.UpdatePlannedStartDate(command.UpdateFeatureDto.PlannedStartDate, updatedByUser.Id, feature.PlannedStartDate);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.PlannedEndDate)
                        feature.UpdatePlannedEndDate(command.UpdateFeatureDto.PlannedEndDate, updatedByUser.Id, feature.PlannedEndDate);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.ActualStartDate)
                        feature.UpdateActualStartDate(command.UpdateFeatureDto.ActualStartDate);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.ActualEndDate)
                        feature.UpdateActualEndDate(command.UpdateFeatureDto.ActualEndDate);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Remarks)
                        feature.UpdateRemarks(command.UpdateFeatureDto.Remarks);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.FunctionalTestability)
                        feature.UpdateFunctionalTestability(command.UpdateFeatureDto.FunctionalTestability);

                    await _unitOfWork.CompleteAsync();

                    //_emailService.send();

                    return Result.Success();
                }

                catch (Exception ex)
                {
                    return Result.Failure(ex.Message);
                }             
                
            }

        }
    }
}
