using System.Collections.Generic;

namespace ProductFocusApi.Dtos
{
    public sealed class OrderingInfoDto
    {
        public IList<FeatureOrderDto> FeaturesOrdering { get; set;}
        public long SprintId { get; set; }
    }
    public sealed class FeatureOrderDto
    {
        public long FeatureId { get; set; }
        public long OrderNumber { get; set; }
    }
}
