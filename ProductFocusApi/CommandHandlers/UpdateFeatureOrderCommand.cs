using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class UpdateFeatureOrderCommand : ICommand
    {
        public OrderingInfoDto OrderingInfo { get; }
        public UpdateFeatureOrderCommand(OrderingInfoDto orderingInfo)
        {
            OrderingInfo = orderingInfo;
        }

        internal sealed class UpdateFeatureOrderCommandHandler : ICommandHandler<UpdateFeatureOrderCommand>
        {
            private IFeatureOrderRepository _featureOrderRepository;
            private IUnitOfWork _unitOfWork;
            public UpdateFeatureOrderCommandHandler(IFeatureOrderRepository featureOrderRepository, IUnitOfWork unitOfWork)
            {
                _featureOrderRepository = featureOrderRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(UpdateFeatureOrderCommand command)
            {
                try
                {
                    foreach (var featureOrder in command.OrderingInfo.featuresOrder)
                    {
                        FeatureOrdering order = await _featureOrderRepository.GetByFeatureIdAndCategory(featureOrder.FeatureId, command.OrderingInfo.SprintId, command.OrderingInfo.OrderingCategory);
                        if (order != null)
                        {
                            _featureOrderRepository.Remove(order);
                        }
                        var newFeatureOrder = FeatureOrdering.CreateInstance(featureOrder.FeatureId, featureOrder.OrderNumber, command.OrderingInfo.SprintId, command.OrderingInfo.OrderingCategory);
                        _featureOrderRepository.Add(newFeatureOrder);
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
