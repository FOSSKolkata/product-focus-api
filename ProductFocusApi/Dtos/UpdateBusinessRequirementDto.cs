using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;

namespace ProductFocusApi.Dtos
{
    public sealed class UpdateBusinessRequirementDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReceivedOn { get; set; }
        public IList<long> TagIds { get; set; }
        public BusinessRequirementSourceEnum SourceEnum { get; set; }
        public string SourceAdditionalInformation { get; set; }
        public string Description { get; set; }
    }
}
