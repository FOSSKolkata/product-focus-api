using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.Dtos
{
    public sealed class OrderingInfoDto
    {
        public IList<FeatureOrderDto> featuresOrdering { get; set;}
        public OrderingCategoryEnum OrderingCategory { get; set; }
        public long SprintId { get; set; }
    }
    public sealed class FeatureOrderDto
    {
        public long FeatureId { get; set; }
        public long OrderNumber { get; set; }
    }
}
