using BusinessRequirements.Domain.Common;
using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Model.BusinessAggregate;
using BusinessRequirements.Domain.Repositories;
using CSharpFunctionalExtensions;
using MediatR;

namespace BusinessRequirements.CommandHandlers
{
    public class AddBusinessRequirementCommand : IRequest<Result>
    {
        public long Id { get; private set; }
        public long ProductId { get; private set; }
        public string Title { get; private set; }
        public DateTime ReceivedOn { get; private set; }
        public IList<long> TagIds { get; private set; }
        public BusinessRequirementSourceEnum SourceEnum { get; private set; }
        public string SourceAdditionalInformation { get; private set; }
        public string Description { get; private set; }
        public AddBusinessRequirementCommand(
            string title,
            long productId,
            DateTime receivedOn,
            IList<long> tagIds,
            BusinessRequirementSourceEnum sourceEnum,
            string sourceAdditionalInformation,
            string description)
        {
            Title = title;
            ProductId = productId;
            ReceivedOn = receivedOn;
            TagIds = tagIds;
            SourceEnum = sourceEnum;
            SourceAdditionalInformation = sourceAdditionalInformation;
            Description = description;
        }
        internal sealed class AddBusinessRequirementCommandHandler : IRequestHandler<AddBusinessRequirementCommand, Result>
        {
            private readonly IBusinessRequirementRepository _businessRequirementRepository;
            private readonly IBusinessRequirementTagRepository _businessRequirementTagRepository;
            private readonly ITagRepository _tagRepository;
            private readonly IProductRepository _productRepository;
            
            private readonly IUnitOfWork _unitOfWork;
            public AddBusinessRequirementCommandHandler(IBusinessRequirementRepository businessRequirementRepository,
                IUnitOfWork unitOfWork, ITagRepository tagRepository,
                IBusinessRequirementTagRepository businessRequirementTagRepository,
                IProductRepository productRepository)
            {
                _businessRequirementRepository = businessRequirementRepository;
                _unitOfWork = unitOfWork;
                _tagRepository = tagRepository;
                _businessRequirementTagRepository = businessRequirementTagRepository;
                _productRepository = productRepository;
            }
            public async Task<Result> Handle(AddBusinessRequirementCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    /*var containerClient = _blobServiceClient.GetBlobContainerClient("productfocusattachments");
                    var attachments = new List<BusinessRequirementAttachment>();
                    foreach (var file in command.Files)
                    {
                        var blobClient = containerClient.GetBlobClient(file.Name + Path.GetExtension(file.Name));
                        await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders { ContentType = file.ContentType });
                        var attachment = BusinessRequirementAttachment.CreateInstance(blobClient.Name, blobClient.Uri.ToString());
                        attachments.Add(attachment);
                    }*/
                    Product product = await _productRepository.GetById(request.ProductId);

                    BusinessRequirement businessRequirement = 
                        BusinessRequirement.CreateInstance(request.Title,
                        product,
                        request.SourceEnum,
                        request.SourceAdditionalInformation,
                        request.Description,
                        request.ReceivedOn).Value;

                    _businessRequirementRepository.Add(businessRequirement);
                    foreach(long tagId in request.TagIds)
                    {
                        Tag tag = await _tagRepository.GetById(tagId);
                        BusinessRequirementTag businessRequirementTag = 
                            BusinessRequirementTag.CreateInstance(
                            businessRequirement.Id, tag).Value;

                        _businessRequirementTagRepository.Add(businessRequirementTag);
                    }
                    request.Id = businessRequirement.Id;
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
