using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProductFocusApi.Dtos
{
    public sealed class GetBusinessRequirementDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReceivedOn { get; set; }
        public List<BusinessRequirementTagDto> Tags { get; set;}
        public BusinessRequirementSourceEnum SourceEnum { get; set; }
        public string SourceInformation { get; set; }
        public string Description { get; set; }
    }

}
