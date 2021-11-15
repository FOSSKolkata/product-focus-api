using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AddFeatureCommand : ICommand
    {
        public long Id { get; }
        public string Title { get; }
        public string WorkItemType { get; set; }
        public long SprintId { get; set; }
        public string IdpUserId { get; set; }

        public AddFeatureCommand(long id, string title, string workItemType, long sprintId, string idpUserId)
        {
            Id = id;
            Title = title;
            WorkItemType = workItemType;
            SprintId = sprintId;
            IdpUserId = idpUserId;
        }

        internal sealed class AddFeatureCommandHandler : ICommandHandler<AddFeatureCommand>
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
            public async Task<Result> Handle(AddFeatureCommand command)
            {
                Product product = await _productRepository.GetById(command.Id);
                if (product == null)
                    return Result.Failure($"No product found with product Id '{command.Id}'");

                Sprint sprint = await _sprintRepository.GetById(command.SprintId);
                if (sprint == null)
                    return Result.Failure($"Invalid sprint id : '{command.SprintId}'");



                bool success = Enum.TryParse(command.WorkItemType, out WorkItemType workItemType);
                if (!success)
                    return Result.Failure($"Work item type '{command.WorkItemType}' is incorrect");

                try
                {
                    User updatedByUser = _userRepository.GetByIdpUserId(command.IdpUserId);
                    var feature = Feature.CreateInstance(product, command.Title, workItemType, sprint, updatedByUser.Id);
                    
                    _featureRepository.AddFeature(feature);
                    foreach (OrderingCategoryEnum orderingCategoryEnum in Enum.GetValues(typeof(OrderingCategoryEnum)))
                    {
                        // FeatureId is not generated, getting 0.
                        FeatureOrdering featureOrdering = FeatureOrdering.CreateInstance(feature.Id, long.MaxValue, feature.Sprint.Id, orderingCategoryEnum);
                        _featureOrderingRepository.Add(featureOrdering);
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
