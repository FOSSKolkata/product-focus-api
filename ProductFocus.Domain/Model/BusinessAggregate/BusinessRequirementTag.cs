using CSharpFunctionalExtensions;
using ProductFocus.Domain.Common;

namespace ProductFocus.Domain.Model.BusinessAggregate
{
    public class BusinessRequirementTag : AggregateRoot<long>
    {
        public virtual long BusinessRequirementId { get; private set; }
        public virtual Tag Tag { get; private set; }
        public virtual long TagId { get; private set; }
        protected BusinessRequirementTag()
        {
            // this protected constructor is for lazy loading to work
        }
        private BusinessRequirementTag(long businessRequirementId, Tag tag)
        {
            BusinessRequirementId = businessRequirementId;
            Tag = tag;
            TagId = tag.Id;
        }
        public static Result<BusinessRequirementTag> CreateInstance(long businessRequirementId, Tag tag)
        {
            BusinessRequirementTag businessRequirementTag = new(businessRequirementId, tag);
            return businessRequirementTag;
        }
    }
}
