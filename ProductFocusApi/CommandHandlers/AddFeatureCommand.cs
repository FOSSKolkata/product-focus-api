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
        
        public AddFeatureCommand(long id, string title, string workItemType)
        {
            Id = id;
            Title = title;
            WorkItemType = workItemType;
        }

        internal sealed class AddFeatureCommandHandler : ICommandHandler<AddFeatureCommand>
        {
            private readonly IProductRepository _productRepository;
            private readonly IFeatureRepository _featureRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public AddFeatureCommandHandler(
                IProductRepository productRepository, IFeatureRepository featureRepository,
                IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _productRepository = productRepository;
                _featureRepository = featureRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(AddFeatureCommand command)
            {
                Module module = await _productRepository.GetModuleById(command.Id);
                if (module == null)
                    return Result.Failure($"No module found with Module Id '{command.Id}'");

                bool success = Enum.TryParse(command.WorkItemType, out WorkItemType workItemType);
                if (!success)
                    return Result.Failure($"Work item type '{command.WorkItemType}' is incorrect");

                try
                {
                    var feature = Feature.CreateInstance(module, command.Title, workItemType);
                    _featureRepository.AddFeature(feature);
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
