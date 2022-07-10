using ProductFocus.Domain.Common;

namespace ProductFocus.Domain.Model
{
    public class BusinessRequirementToWorkItemLink: AggregateRoot<long>
    {
        public long WorkItemId { get; private set; }
	    public long BusinessRequirementId { get; private set; }

        protected BusinessRequirementToWorkItemLink()
        {

        }

        private BusinessRequirementToWorkItemLink(long workItemId, long businessRequirementId)
        {
            WorkItemId = workItemId;
            BusinessRequirementId = businessRequirementId;
        }

        public static BusinessRequirementToWorkItemLink CreateInstance(long workItemId, long businessRequirementId)
        {
            return new(workItemId, businessRequirementId);
        }
    }
}
