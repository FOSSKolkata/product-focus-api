using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class UpdateFeatureOrderingCommand : ICommand
    {
        public OrderingInfoDto OrderingInfo { get; }
        public UpdateFeatureOrderingCommand(OrderingInfoDto orderingInfo)
        {
            OrderingInfo = orderingInfo;
        }

        internal sealed class UpdateFeatureOrderCommandHandler : ICommandHandler<UpdateFeatureOrderingCommand>
        {
            private readonly IFeatureOrderingRepository _featureOrderRepository;
            private readonly IUnitOfWork _unitOfWork;
            public UpdateFeatureOrderCommandHandler(IFeatureOrderingRepository featureOrderRepository, IUnitOfWork unitOfWork)
            {
                _featureOrderRepository = featureOrderRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(UpdateFeatureOrderingCommand command)
            {
                try
                {
                    List<FeatureOrdering> featureOrderings = await _featureOrderRepository.GetByCategoryAndSprint(command.OrderingInfo.SprintId);
                    foreach(var featureOrdering in featureOrderings)
                    {
                        _featureOrderRepository.Remove(featureOrdering);
                    }
                    foreach (var featureOrdering in command.OrderingInfo.FeaturesOrdering)
                    {
                        var newFeatureOrdering = FeatureOrdering.CreateInstance(featureOrdering.FeatureId, featureOrdering.OrderNumber, command.OrderingInfo.SprintId);
                        _featureOrderRepository.Add(newFeatureOrdering);
                    }
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
