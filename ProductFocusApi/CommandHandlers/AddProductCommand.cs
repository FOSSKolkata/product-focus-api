using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocus.AppServices
{
    public sealed class AddProductCommand : ICommand
    {
        public long Id { get; }
        public string Name { get; }
        public AddProductCommand(long id, string name)
        {
            Id = id;
            Name = name;
        }

        internal sealed class AddProductCommandHandler : ICommandHandler<AddProductCommand>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailService _emailService;

            public AddProductCommandHandler(
                IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IEmailService emailService,
                IProductRepository productRepository)
            {
                _organizationRepository = organizationRepository;
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
                _emailService = emailService;
            }
            public async Task<Result> Handle(AddProductCommand command)
            {
                Organization organization = await _organizationRepository.GetById(command.Id);                
                if (organization == null)
                    return Result.Failure($"No Organization found with Id '{command.Id}'");

                bool ifProductExists = organization.IfProductExists(command.Name);
                if (ifProductExists)
                    return Result.Failure($"Product '{command.Name}' already present");

                var product = new Product(organization, command.Name);
                _productRepository.AddProduct(product);
                await _unitOfWork.CompleteAsync();
                
                _emailService.send();
                
                return Result.Success();
                            
            }

        }
    }
}
