using Common;

namespace ProductFocus.Domain.Model.BusinessAggregate
{
    public class BusinessRequirementAttachment : AggregateRoot<long>
    {
        public virtual string Name { get; private set; }
        public virtual string Uri { get; private set; }
        protected BusinessRequirementAttachment()
        {

        }
        private BusinessRequirementAttachment(string name, string uri)
        {
            Name = name;
            Uri = uri;
        }
        public static BusinessRequirementAttachment CreateInstance(string name, string uri)
        {
            BusinessRequirementAttachment businessRequirementAttachment = new(name, uri);
            return businessRequirementAttachment;
        }
    }
}
