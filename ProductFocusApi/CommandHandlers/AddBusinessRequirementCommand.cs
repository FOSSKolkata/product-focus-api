using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Model.BusinessAggregate;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProductFocusApi.CommandHandlers
{
    public class AddBusinessRequirementCommand : ICommand
    {
        public long ProductId { get; private set; }
        public DateTime Date { get; private set; }
        public IList<long> TagIds { get; private set; }
        public string Source { get; private set; }
        public string SourceAdditionalInformation { get; private set; }
        public string Description { get; private set; }
        public IList<IFormFile> Files { get; private set; }
        public AddBusinessRequirementCommand(DateTime date,
            IList<long> tagIds,
            string source,
            string sourceAdditionalInformation,
            string description,
            IList<IFormFile> files)
        {
            Date = date;
            TagIds = tagIds;
            Source = source;
            SourceAdditionalInformation = sourceAdditionalInformation;
            Description = description;
            Files = files;
        }
        internal sealed class AddBusinessRequirementCommandHandler : ICommandHandler<AddBusinessRequirementCommand>
        {
            private readonly IBusinessRequirementRepository _businessRequirementRepository;
            private readonly IBusinessRequirementTagRepository _businessRequirementTagRepository;
            private readonly ITagRepository _tagRepository;
            private readonly BlobServiceClient _blobServiceClient;
            
            private readonly IUnitOfWork _unitOfWork;
            public AddBusinessRequirementCommandHandler(IBusinessRequirementRepository businessRequirementRepository,
                IUnitOfWork unitOfWork, ITagRepository tagRepository,
                IBusinessRequirementTagRepository businessRequirementTagRepository,
                BlobServiceClient blobServiceClient)
            {
                _businessRequirementRepository = businessRequirementRepository;
                _unitOfWork = unitOfWork;
                _tagRepository = tagRepository;
                _businessRequirementTagRepository = businessRequirementTagRepository;
                _blobServiceClient = blobServiceClient;
            }
            public async Task<Result> Handle(AddBusinessRequirementCommand command)
            {
                try
                {
                    var containerClient = _blobServiceClient.GetBlobContainerClient("productfocusattachments");
                    var attachments = new List<BusinessRequirementAttachment>();
                    foreach (var file in command.Files)
                    {
                        var blobClient = containerClient.GetBlobClient(file.Name + Path.GetExtension(file.Name));
                        await blobClient.UploadAsync(file.OpenReadStream(), new BlobHttpHeaders { ContentType = file.ContentType });
                        var attachment = BusinessRequirementAttachment.CreateInstance(blobClient.Name, blobClient.Uri.ToString());
                        attachments.Add(attachment);
                    }
                    BusinessRequirement businessRequirement = BusinessRequirement.CreateInstance(command.ProductId,
                        command.Source, command.SourceAdditionalInformation, command.Description, command.Date, attachments).Value;
                    _businessRequirementRepository.Add(businessRequirement);
                    foreach(long tagId in command.TagIds)
                    {
                        Tag tag = await _tagRepository.GetById(tagId);
                        BusinessRequirementTag businessRequirementTag = BusinessRequirementTag.CreateInstance(businessRequirement.Id, tag).Value;
                        _businessRequirementTagRepository.Add(businessRequirementTag);
                    }
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
