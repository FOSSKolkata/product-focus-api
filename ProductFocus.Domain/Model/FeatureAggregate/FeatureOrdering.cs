using ProductFocus.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductFocus.Domain.Model
{
    [Table("FeatureOrderings")]
    public class FeatureOrdering : AggregateRoot<Guid>
    {
        public long FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
        public long OrderNumber { get; set; }
        public long? SprintId { get; set; }

        protected FeatureOrdering()
        {

        }
        public static FeatureOrdering CreateInstance(long featureId, long orderNumber, long? sprintId)
        {
            FeatureOrdering featureOrder = new();
            featureOrder.FeatureId = featureId;
            featureOrder.OrderNumber = orderNumber;
            featureOrder.SprintId = sprintId;
            return featureOrder;
        }

        public virtual void UpdateSprint(long sprintId)
        {
            this.SprintId = sprintId;
            this.OrderNumber = long.MaxValue; // Add the feature to fag end of the sprint work items.
        }
    }
}
