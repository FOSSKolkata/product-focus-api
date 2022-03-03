using ProductFocus.Domain.Common;

namespace ProductFocus.Domain.Model.BusinessAggregate
{
    public class BusinessRequirementAttachment : AggregateRoot<long>
    {
        public virtual long BusinessRequirementId { get; private set; }
        public virtual string Name { get; private set; }
        public virtual string Uri { get; private set; }
        public virtual string FileName { get; private set; }
        protected BusinessRequirementAttachment()
        {

        }
        private BusinessRequirementAttachment(long businessRequiremenId, string name, string uri, string fileName)
        {
            BusinessRequirementId = businessRequiremenId;
            Name = name;
            Uri = uri;
            FileName = fileName;
        }
        public static BusinessRequirementAttachment CreateInstance(long businessRequirementId, string name, string uri, string fileName)
        {
            BusinessRequirementAttachment businessRequirementAttachment = new(businessRequirementId, name, uri, fileName);
            return businessRequirementAttachment;
        }
    }
}
