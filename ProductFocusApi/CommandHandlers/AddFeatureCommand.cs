using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocus.AppServices
{
    public sealed class AddFeatureCommand : IRequest<Result>
    {
        public long Id { get; }
        public string Title { get; }
        public string WorkItemType { get; set; }
        public long? SprintId { get; set; }
        public string IdpUserId { get; set; }

        public AddFeatureCommand(long id, string title, string workItemType, long? sprintId, string idpUserId)
        {
            Id = id;
            Title = title;
            WorkItemType = workItemType;
            SprintId = sprintId;
            IdpUserId = idpUserId;
        }

        internal sealed class AddFeatureCommandHandler : IRequestHandler<AddFeatureCommand, Result>
        {
            private readonly IProductRepository _productRepository;
            private readonly IFeatureRepository _featureRepository;
            private readonly ISprintRepository _sprintRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserRepository _userRepository;
            private readonly IFeatureOrderingRepository _featureOrderingRepository;

            public AddFeatureCommandHandler(
                IProductRepository productRepository, IFeatureRepository featureRepository,
                ISprintRepository sprintRepository,
                IUnitOfWork unitOfWork,
                IUserRepository userRepository,
                IFeatureOrderingRepository featureOrderingRepository)
            {
                _productRepository = productRepository;
                _featureRepository = featureRepository;
                _sprintRepository = sprintRepository;
                _unitOfWork = unitOfWork;
                _userRepository = userRepository;
                _featureOrderingRepository = featureOrderingRepository;
            }
            public async Task<Result> Handle(AddFeatureCommand request, CancellationToken cancellationToken)
            {
                Product product = await _productRepository.GetById(request.Id);
                if (product == null)
                    return Result.Failure($"No product found with product Id '{request.Id}'");

                Sprint sprint = request.SprintId is null ? null : await _sprintRepository.GetById((long)request.SprintId);
                /*if (sprint == null)
                    return Result.Failure($"Invalid sprint id : '{request.SprintId}'");*/



                bool success = Enum.TryParse(request.WorkItemType, out WorkItemType workItemType);
                if (!success)
                    return Result.Failure($"Work item type '{request.WorkItemType}' is incorrect");

                try
                {
                    User updatedByUser = _userRepository.GetByIdpUserId(request.IdpUserId);
                    var feature = Feature.CreateInstance(product, request.Title, workItemType, sprint, updatedByUser.Id);
                    
                    _featureRepository.AddFeature(feature);
                    FeatureOrdering featureOrdering = FeatureOrdering.CreateInstance(feature.Id, long.MaxValue, feature.Sprint?.Id);
                    _featureOrderingRepository.Add(featureOrdering);
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
