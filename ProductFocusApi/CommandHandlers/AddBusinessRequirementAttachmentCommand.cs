using Azure.Storage.Blobs;
using ProductFocus.Domain.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using ProductFocus.Domain.Services;
using ProductFocusApi.ConnectionString;
using System;
using System.Threading.Tasks;
using MediatR;
using System.Threading;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class AddBusinessRequirementAttachmentCommand : IRequest<Result>
    {
        public long BusinessRequirementId { get; private set; }
        public IFormFileCollection Attachments { get; private set; }

        public AddBusinessRequirementAttachmentCommand(long businessRequirementId, IFormFileCollection attachments)
        {
            BusinessRequirementId = businessRequirementId;
            Attachments = attachments;
        }
        public sealed class AddBusinessRequirementAttachmentCommandHandler : IRequestHandler<AddBusinessRequirementAttachmentCommand, Result>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IBusinessRequirementRepository _businessRequirementRepository;
            private readonly BlobServiceClient _blobServiceClient;
            private readonly BusinessRequirementContainerName _businessRequirementContainerName;
            private readonly IBlobStorageService _blobStorageService;
            private readonly IProductRepository _productRepository;

            public AddBusinessRequirementAttachmentCommandHandler(IUnitOfWork unitOfWork,
                IBusinessRequirementRepository businessRequirementRepository,
                BlobServiceClient blobServiceClient,
                BusinessRequirementContainerName businessRequirementContainerName,
                IBlobStorageService blobStorageService,
                IProductRepository productRepository)
            {
                _unitOfWork = unitOfWork;
                _businessRequirementRepository = businessRequirementRepository;
                _blobServiceClient = blobServiceClient;
                _businessRequirementContainerName = businessRequirementContainerName;
                _blobStorageService = blobStorageService;
                _productRepository = productRepository;
            }

            public async Task<Result> Handle(AddBusinessRequirementAttachmentCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    BusinessRequirement businessRequirement = await _businessRequirementRepository.GetById(request.BusinessRequirementId);
                    BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_businessRequirementContainerName.Value);
                    Product product = await _productRepository.GetById(businessRequirement.ProductId);

                    foreach (var file in request.Attachments)
                    {
                        BlobClient blobClient = await _blobStorageService.AddAsync(BlobStorageFileTypeEnum.BusinessRequirementAttachments,
                            product.Organization.Id, product.Id, request.BusinessRequirementId, file);
                        businessRequirement.AddAttachment(blobClient.Name, blobClient.Uri.ToString(), file.FileName);
                    }
                    await _unitOfWork.CompleteAsync(cancellationToken);
                }
                    catch(Exception ex)
                {
                    return Result.Failure(ex.Message);
                }
                return Result.Success();
            }
        }
    }
}
