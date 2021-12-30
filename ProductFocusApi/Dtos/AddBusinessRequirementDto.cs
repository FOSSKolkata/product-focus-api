using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ProductFocusApi.Dtos
{
    public sealed class AddBusinessRequirementDto
    {
        public long ProductId { get; set; }
        public DateTime Date { get; set; }
        public IList<long> TagIds { get; set; }
        public string Source { get; set; }
        public string SourceAdditionalInformation { get; set; }
        public string Description { get; set; }
        public IList<IFormFile> Files { get; set; }
    }
}
