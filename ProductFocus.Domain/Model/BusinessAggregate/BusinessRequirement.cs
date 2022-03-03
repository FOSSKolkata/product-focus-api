using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model.BusinessAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductFocus.Domain.Model
{
    public class BusinessRequirement : AggregateRoot<long>, ISoftDeletable
    {
        public virtual Product Product { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual string Title { get; private set; }
        public virtual DateTime ReceivedOn { get; private set; }
        public virtual BusinessRequirementSourceEnum SourceEnum { get; private set; }
        public virtual string SourceInformation { get; private set; }
        public virtual string Description { get; private set; }
        public readonly IList<BusinessRequirementAttachment> _businessRequirementAttachments = new List<BusinessRequirementAttachment>();
        public virtual IReadOnlyList<BusinessRequirementAttachment> BusinessRequirementAttachments => _businessRequirementAttachments.ToList();
        public bool IsDeleted { get; set; }
        public DateTime DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        protected BusinessRequirement()
        {
            // this protected constructor is for lazy loading to work
        }
        private BusinessRequirement(string title, DateTime receivedOn,
            BusinessRequirementSourceEnum sourceEnum, string sourceInformation,
            string description, Product product)
        {
            Title = title;
            ReceivedOn = receivedOn;
            SourceEnum = sourceEnum;
            SourceInformation = sourceInformation;
            Description = description;
            ProductId = product.Id;
            Product = product;
        }
        public static Result<BusinessRequirement> CreateInstance(string title, Product product,
            BusinessRequirementSourceEnum sourceEnum, string sourceInformation, string description, DateTime receivedOn)
        {
            BusinessRequirement businessRequirement = new(title, receivedOn, sourceEnum, sourceInformation, description, product);
            return businessRequirement;
        }
        public void UpdateTitle(string title)
        {
            Title = title;
        }
        public void UpdateReceivedOn(DateTime receivedOn)
        {
            ReceivedOn = receivedOn;
        }
        public void UpdateSourceEnum(BusinessRequirementSourceEnum sourceEnum)
        {
            SourceEnum = sourceEnum;
        }
        public void UpdateSourceInformation(string sourceInformation)
        {
            SourceInformation = sourceInformation;
        }
        public void UpdateDescription(string description)
        {
            Description = description;
        }
        public void AddAttachment(string name, string uri, string fileName)
        {
            BusinessRequirementAttachment businessRequirementAttachment = BusinessRequirementAttachment.CreateInstance(Id, name, uri, fileName);
            _businessRequirementAttachments.Add(businessRequirementAttachment);
        }

        public Result DeleteAttachment(long id)
        {
            var attachmentToBeDeleted = GetAttachmentByAttachmentId(id);

            if (attachmentToBeDeleted == null)
                return Result.Failure("Attachment not found: " + nameof(BusinessRequirementAttachment));

            _businessRequirementAttachments.Remove(attachmentToBeDeleted);
            return Result.Success();
        }

        public void Delete(string userId)
        {
            IsDeleted = true;
            DeletedOn = DateTime.Now;
            DeletedBy = userId;
        }

        public BusinessRequirementAttachment GetAttachmentByAttachmentId(long id)
        {
            return this.BusinessRequirementAttachments.Where(x => x.Id == id).SingleOrDefault();
        }
    }

    public enum BusinessRequirementSourceEnum
    {
        Email = 1,
        Meeting = 2
    }
}
