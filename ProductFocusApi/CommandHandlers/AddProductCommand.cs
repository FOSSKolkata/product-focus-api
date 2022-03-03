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
    public sealed class AddProductCommand : IRequest<Result>
    {
        public long Id { get; }
        public string Name { get; }
        public AddProductCommand(long id, string name)
        {
            Id = id;
            Name = name;
        }

        internal sealed class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AddProductCommandHandler(
                IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork,
                IProductRepository productRepository)
            {
                _organizationRepository = organizationRepository;
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
            {
                Organization organization = await _organizationRepository.GetById(request.Id);                
                if (organization == null)
                    return Result.Failure($"No Organization found with Id '{request.Id}'");

                bool ifProductExists = organization.IfProductExists(request.Name);
                if (ifProductExists)
                    return Result.Failure($"Product '{request.Name}' already present");

                try
                {
                    var product = Product.CreateInstance(organization, request.Name);
                    _productRepository.AddProduct(product);
                    await _unitOfWork.CompleteAsync(cancellationToken);

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
