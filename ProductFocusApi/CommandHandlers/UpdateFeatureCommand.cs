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
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public UpdateFeatureCommandHandler(
                IProductRepository productRepository, IFeatureRepository featureRepository,
                IUnitOfWork unitOfWork, IEmailService emailService)
            {                
                _featureRepository = featureRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(UpdateFeatureCommand command)
            {
                Feature feature = await _featureRepository.GetById(command.UpdateFeatureDto.Id);
                
                if (feature == null)
                    return Result.Failure($"No feature found with Feature Id '{command.UpdateFeatureDto.Id}'");                

                if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Title)
                    feature.UpdateTitle(command.UpdateFeatureDto.Title);

                if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Description)
                    feature.UpdateTitle(command.UpdateFeatureDto.Description);

                if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.WorkCompletionPercentage)
                    feature.UpdateWorkCompletionPercentage(command.UpdateFeatureDto.WorkCompletionPercentage);

                if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.Status)
                    feature.UpdateStatus(command.UpdateFeatureDto.Status);

                if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.StoryPoint)
                    feature.UpdateStoryPoint(command.UpdateFeatureDto.StoryPoint);

                if (command.UpdateFeatureDto.FieldName == UpdateColumnIdentifier.IsBlocked)
                    feature.UpdateBlockedStatus(command.UpdateFeatureDto.IsBlocked);

                try
                {                    
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
