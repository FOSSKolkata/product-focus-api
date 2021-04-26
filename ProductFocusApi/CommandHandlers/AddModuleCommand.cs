using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
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
            private readonly IEmailService _emailService;

            public AddModuleCommandHandler(
                IProductRepository productRepository, IUnitOfWork unitOfWork, IEmailService emailService)
            {
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(AddModuleCommand command)
            {
                Product product = await _productRepository.GetById(command.Id);
                if (product == null)
                    return Result.Failure($"No product found with Id '{command.Id}'");
                
                try
                {
                    product.AddModule(command.Name);
                    await _unitOfWork.CompleteAsync();
                
                    _emailService.send();
                    
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
