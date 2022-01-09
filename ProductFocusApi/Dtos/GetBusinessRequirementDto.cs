using System;
using System.Collections.Generic;

namespace ProductFocusApi.Dtos
{
    public sealed class GetBusinessRequirementDto
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Title { get; set; }
        public DateTime ReceivedOn { get; set; }
        public IList<BusinessRequirementTagDto> Tags { get; set; }
    }

    public sealed class BusinessRequirementTagDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long BusinessRequirementId { get; set; }
    }
}
