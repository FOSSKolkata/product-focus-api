using Azure;
using ProductFocus.Domain.Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Model.BusinessAggregate;
using ProductFocus.Domain.Repositories;
using ProductFocus.Domain.Services;
using System;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public sealed class DeleteBusinessRequirementAttachmentCommand : ICommand
    {
        public long BusinessRequirementId { get; private set; }
        public long AttachmentId { get; private set; }
        public DeleteBusinessRequirementAttachmentCommand(long businessRequirementId, long attachmentId)
        {
            BusinessRequirementId = businessRequirementId;
            AttachmentId = attachmentId;
        }

        public sealed class DeleteBusinessRequirementAttachmentCommandHandler : ICommandHandler<DeleteBusinessRequirementAttachmentCommand>
        {
            private readonly IBusinessRequirementRepository _businessRequirementRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IBlobStorageService _blobStorageService;
            private readonly IProductRepository _productRepository;

            public DeleteBusinessRequirementAttachmentCommandHandler(
                IBusinessRequirementRepository businessRequirementRepository,
                IUnitOfWork unitOfWork,
                IBlobStorageService blobStorageService,
                IProductRepository productRepository)
            {
                _businessRequirementRepository = businessRequirementRepository;
                _unitOfWork = unitOfWork;
                _blobStorageService = blobStorageService;
                _productRepository = productRepository;
            }
            public async Task<Result> Handle(DeleteBusinessRequirementAttachmentCommand command)
            {
                try
                {
                    BusinessRequirement businessRequirement = await _businessRequirementRepository.GetById(command.BusinessRequirementId);
                    BusinessRequirementAttachment attachmentToBeDeleted = businessRequirement.GetAttachmentByAttachmentId(command.AttachmentId);
                    Product product = await _productRepository.GetById(businessRequirement.ProductId);
                    Response blobClient = await _blobStorageService.DeleteAsync(BlobStorageFileTypeEnum.BusinessRequirementAttachments, attachmentToBeDeleted.Name);
                    Result result = businessRequirement.DeleteAttachment(command.AttachmentId);

                    if (result.IsFailure)
                        return result;

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
