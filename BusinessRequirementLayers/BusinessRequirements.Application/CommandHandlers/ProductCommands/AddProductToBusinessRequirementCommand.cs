using BusinessRequirements.Domain.Common;
using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Repositories;
using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Application.CommandHandlers.ProductCommands
{
    public class AddProductToBusinessRequirementCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public long OrganizationId { get; private set; }
        public string Name { get; private set; }
        public AddProductToBusinessRequirementCommand(long id, long organizationId, string name)
        {
            Id = id;
            OrganizationId = organizationId;
            Name = name;
        }

        internal sealed class AddProductToBusinessRequirementCommandHandler : IRequestHandler<AddProductToBusinessRequirementCommand, Result>
        {
            private readonly IOrganizationRepository _organizationRepository;
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;

            public AddProductToBusinessRequirementCommandHandler(
                IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork,
                IProductRepository productRepository)
            {
                _organizationRepository = organizationRepository;
                _productRepository = productRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(AddProductToBusinessRequirementCommand request, CancellationToken cancellationToken)
            {
                Organization organization = await _organizationRepository.GetById(request.OrganizationId);
                try
                {
                    var product = Product.CreateInstance(request.Id,organization, request.Name);
                    _productRepository.Add(product);

                    await _unitOfWork.CompleteAsync(cancellationToken);
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