using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AddModuleCommand : ICommand
    {
        public long Id { get; }
        public string Name { get; }
        public AddModuleCommand(long id, string name)
        {
            Id = id;
            Name = name;
        }

        internal sealed class AddModuleCommandHandler : ICommandHandler<AddModuleCommand>
        {
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IModuleRepository _moduleRepository;

            public AddModuleCommandHandler(
                IProductRepository productRepository,
                IUnitOfWork unitOfWork,
                IModuleRepository moduleRepository)
            {
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
                _moduleRepository = moduleRepository;
            }
            public async Task<Result> Handle(AddModuleCommand command)
            {
                Product product = await _productRepository.GetById(command.Id);
                if (product == null)
                    return Result.Failure($"No product found with Id '{command.Id}'");
                Module module = await _moduleRepository.GetByName(command.Name);
                if (module != null)
                    return Result.Failure($"Module already exist with the name '{command.Name}'");
                try
                {
                    product.AddModule(command.Name);
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
