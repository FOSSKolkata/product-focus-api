using Common;
using CSharpFunctionalExtensions;
using ProductFocus.Domain.Model.BusinessAggregate;
using System;
using System.Collections.Generic;

namespace ProductFocus.Domain.Model
{
    public class BusinessRequirement : AggregateRoot<long>
    {
        public virtual long ProductId { get; private set; }
        public virtual DateTime Date { get; private set; }
        public virtual string Source { get; private set; }
        public virtual string SourceInformation { get; private set; }
        public virtual string Description { get; private set; }
        public virtual IList<BusinessRequirementAttachment> Attachments { get; private set; }
        protected BusinessRequirement()
        {
            // this protected constructor is for lazy loading to work
        }
        private BusinessRequirement(DateTime date,
            string source, string sourceInformation,
            string description, long productId, IList<BusinessRequirementAttachment> attachments)
        {
            Date = date;
            Source = source;
            SourceInformation = sourceInformation;
            Description = description;
            ProductId = productId;
            Attachments = attachments;
        }
        public static Result<BusinessRequirement> CreateInstance(long productId,
            string source, string sourceInformation, string description, DateTime date,
            IList<BusinessRequirementAttachment> attachments)
        {
            BusinessRequirement businessRequirement = new(date, source, sourceInformation, description, productId, attachments);
            return businessRequirement;
        }
    }
}
