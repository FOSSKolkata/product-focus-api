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

        public UpdateFeatureCommand(UpdateFeatureDto updateFeatureDto)
        {
            UpdateFeatureDto = updateFeatureDto;
        }

        internal sealed class UpdateFeatureCommandHandler : ICommandHandler<UpdateFeatureCommand>
        {            
            private readonly IFeatureRepository _featureRepository;
            private readonly ISprintRepository _sprintRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public UpdateFeatureCommandHandler(
                IProductRepository productRepository, IFeatureRepository featureRepository,
                ISprintRepository sprintRepository, IUserRepository userRepository, 
                IUnitOfWork unitOfWork, IEmailService emailService)
            {                
                _featureRepository = featureRepository;
                _sprintRepository = sprintRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(UpdateFeatureCommand command)
            {
                Feature feature = await _featureRepository.GetById(command.UpdateFeatureDto.Id);
                
                if (feature == null)
                    return Result.Failure($"No feature found with Feature Id '{command.UpdateFeatureDto.Id}'");                
                                

                try
                {
                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Title)
                        feature.UpdateTitle(command.UpdateFeatureDto.Title);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Description)
                        feature.UpdateDescription(command.UpdateFeatureDto.Description);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.WorkCompletionPercentage)
                        feature.UpdateWorkCompletionPercentage(command.UpdateFeatureDto.WorkCompletionPercentage);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Status)
                        feature.UpdateStatus(command.UpdateFeatureDto.Status);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Sprint)
                    {
                        Sprint sprintDetails = _sprintRepository.GetByName(command.UpdateFeatureDto.SprintName);

                        if (sprintDetails == null)
                            return Result.Failure($"Sprint with name '{command.UpdateFeatureDto.SprintName}' doesn't exist");

                        feature.UpdateSprint(sprintDetails);
                    }

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.StoryPoint)
                        feature.UpdateStoryPoint(command.UpdateFeatureDto.StoryPoint);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IsBlocked)
                        feature.UpdateBlockedStatus(command.UpdateFeatureDto.IsBlocked);

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IncludeAssignee)
                    {
                        User userDetails = _userRepository.GetByEmail(command.UpdateFeatureDto.EmailOfAssignee);

                        if (userDetails == null)
                            return Result.Failure($"User with email '{command.UpdateFeatureDto.EmailOfAssignee}' doesn't exist");

                        feature.IncludeAssignee(userDetails);
                    }

                    if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.AcceptanceCriteria)
                        feature.UpdateAcceptanceCriteria(command.UpdateFeatureDto.AcceptanceCriteria);


                    await _unitOfWork.CompleteAsync();

                    _emailService.send();

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
