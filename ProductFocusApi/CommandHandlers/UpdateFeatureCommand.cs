using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class UpdateFeatureCommand : IRequest<Result>
    {
        public UpdateFeatureDto UpdateFeatureDto { get; }

        public string IdpUserId { get; }
        public UpdateFeatureCommand(UpdateFeatureDto updateFeatureDto, string idpUserId)
        {
            UpdateFeatureDto = updateFeatureDto;
            IdpUserId = idpUserId;
        }

        internal sealed class UpdateFeatureCommandHandler : IRequestHandler<UpdateFeatureCommand, Result>
        {            
            private readonly IFeatureRepository _featureRepository;
            private readonly ISprintRepository _sprintRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IFeatureOrderingRepository _featureOrderingRepository;
            private readonly IModuleRepository _moduleRepository;

            public UpdateFeatureCommandHandler(
                IFeatureRepository featureRepository,
                ISprintRepository sprintRepository, IUserRepository userRepository, 
                IUnitOfWork unitOfWork,
                IFeatureOrderingRepository featureOrderRepository,
                IModuleRepository moduleRepository)
            {                
                _featureRepository = featureRepository;
                _sprintRepository = sprintRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _featureOrderingRepository = featureOrderRepository;
                _moduleRepository = moduleRepository;
            }
            public async Task<Result> Handle(UpdateFeatureCommand request, CancellationToken cancellationToken)
            {
                Feature feature = await _featureRepository.GetById(request.UpdateFeatureDto.Id);
                
                if (feature == null)
                    return Result.Failure($"No feature found with Feature Id '{request.UpdateFeatureDto.Id}'");                
                                

                try
                {
                    User updatedByUser = _userRepository.GetByIdpUserId(request.IdpUserId);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Title)
                        feature.UpdateTitle(request.UpdateFeatureDto.Title, updatedByUser.Id, feature.Title);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Description)
                        feature.UpdateDescription(request.UpdateFeatureDto.Description, updatedByUser.Id, feature.Description);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.WorkCompletionPercentage)
                        feature.UpdateWorkCompletionPercentage(request.UpdateFeatureDto.WorkCompletionPercentage, updatedByUser.Id, feature.WorkCompletionPercentage);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Status)
                        feature.UpdateStatus(request.UpdateFeatureDto.Status);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Sprint)
                    {
                        Sprint currentSprint = _sprintRepository.GetByName(request.UpdateFeatureDto.SprintName);

                        if (currentSprint == null)
                            return Result.Failure($"Sprint with name '{request.UpdateFeatureDto.SprintName}' doesn't exist");
                        feature.UpdateSprint(currentSprint, updatedByUser.Id, feature.Sprint);

                        List<FeatureOrdering> featureOrderings = await _featureOrderingRepository.GetByIdAndSprint(feature.Id, feature.Sprint.Id);
                        foreach (var featureOrdering in featureOrderings)
                        {
                            featureOrdering.UpdateSprint(currentSprint.Id);
                        }
                    }

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.StoryPoint)
                        feature.UpdateStoryPoint(request.UpdateFeatureDto.StoryPoint, updatedByUser.Id, feature.StoryPoint);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IsBlocked)
                    {
                        feature.UpdateBlockedStatus(request.UpdateFeatureDto.IsBlocked, updatedByUser.Id);
                    }

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IncludeAssignee)
                    {
                        User userDetails = _userRepository.GetByEmail(request.UpdateFeatureDto.EmailOfAssignee);

                        if (userDetails == null)
                            return Result.Failure($"User with email '{request.UpdateFeatureDto.EmailOfAssignee}' doesn't exist");

                        feature.IncludeAssignee(userDetails, updatedByUser.Id, request.UpdateFeatureDto.EmailOfAssignee);
                    }

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.ExcludeAssignee)
                    {
                        User userDetails = _userRepository.GetByEmail(request.UpdateFeatureDto.EmailOfAssignee);

                        if (userDetails == null)
                            return Result.Failure($"User with email '{request.UpdateFeatureDto.EmailOfAssignee}' doesn't exist");

                        feature.ExcludeAssignee(userDetails, updatedByUser.Id, request.UpdateFeatureDto.EmailOfAssignee);
                    }

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IncludeAndExcludeOwners)
                    {
                        foreach(long id in request.UpdateFeatureDto.ExcludeOwnerList)
                        {
                            User user = _userRepository.GetById(id);
                            feature.ExcludeAssignee(user, user.Id, user.Email);
                        }
                        foreach(long id in request.UpdateFeatureDto.IncludeOwnerList)
                        {
                            User user = _userRepository.GetById(id);
                            feature.IncludeAssignee(user, user.Id, user.Email);
                        }
                    }

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.AcceptanceCriteria)
                        feature.UpdateAcceptanceCriteria(request.UpdateFeatureDto.AcceptanceCriteria);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.PlannedStartDate)
                        feature.UpdatePlannedStartDate(request.UpdateFeatureDto.PlannedStartDate, updatedByUser.Id, feature.PlannedStartDate);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.PlannedEndDate)
                        feature.UpdatePlannedEndDate(request.UpdateFeatureDto.PlannedEndDate, updatedByUser.Id, feature.PlannedEndDate);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.ActualStartDate)
                        feature.UpdateActualStartDate(request.UpdateFeatureDto.ActualStartDate);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.ActualEndDate)
                        feature.UpdateActualEndDate(request.UpdateFeatureDto.ActualEndDate);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Remarks)
                        feature.UpdateRemarks(request.UpdateFeatureDto.Remarks);

                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.FunctionalTestability)
                        feature.UpdateFunctionalTestability(request.UpdateFeatureDto.FunctionalTestability);
                    if (request.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.UpdateModule)
                    {
                        Module module = request.UpdateFeatureDto.ModuleId != null ? await _moduleRepository.GetById(request.UpdateFeatureDto.ModuleId??0) : null;
                        feature.UpdateModule(module);
                    }
                    await _unitOfWork.CompleteAsync();


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
