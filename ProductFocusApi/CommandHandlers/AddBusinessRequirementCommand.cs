using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Model.BusinessAggregate;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class AddBusinessRequirementCommand : ICommand
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
        internal sealed class AddBusinessRequirementCommandHandler : ICommandHandler<AddBusinessRequirementCommand>
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
            public async Task<Result> Handle(AddBusinessRequirementCommand command)
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
                    Product product = await _productRepository.GetById(command.ProductId);

                    BusinessRequirement businessRequirement = 
                        BusinessRequirement.CreateInstance(command.Title,
                        product,
                        command.SourceEnum,
                        command.SourceAdditionalInformation,
                        command.Description,
                        command.ReceivedOn).Value;

                    _businessRequirementRepository.Add(businessRequirement);
                    foreach(long tagId in command.TagIds)
                    {
                        Tag tag = await _tagRepository.GetById(tagId);
                        BusinessRequirementTag businessRequirementTag = 
                            BusinessRequirementTag.CreateInstance(
                            businessRequirement.Id, tag).Value;

                        _businessRequirementTagRepository.Add(businessRequirementTag);
                    }
                    command.Id = businessRequirement.Id;
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
