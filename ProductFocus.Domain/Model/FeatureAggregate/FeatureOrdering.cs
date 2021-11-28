﻿using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    [Table("FeatureOrderings")]
    public class FeatureOrdering : AggregateRoot<Guid>
    {
        public OrderingCategoryEnum OrderingCategory { get; set; }
        public long FeatureId { get; set; }
        public virtual Feature Feature { get; set; }
        public long OrderNumber { get; set; }
        public long SprintId { get; set; }

        protected FeatureOrdering()
        {

        }
        public static FeatureOrdering CreateInstance(long featureId, long orderNumber, long sprintId, OrderingCategoryEnum orderingCategory)
        {
            FeatureOrdering featureOrder = new();
            featureOrder.FeatureId = featureId;
            featureOrder.OrderNumber = orderNumber;
            featureOrder.SprintId = sprintId;
            featureOrder.OrderingCategory = orderingCategory;
            return featureOrder;
        }

        public virtual void UpdateSprint(long sprintId)
        {
            this.SprintId = sprintId;
            this.OrderNumber = long.MaxValue; // Add the feature to fag end of the sprint work items.
        }
    }

    public enum OrderingCategoryEnum
    {
        BoardView = 1,
        ScrumView = 2
    }
}
