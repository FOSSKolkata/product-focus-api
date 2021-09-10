using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Dtos
{
    public sealed class GetDomainEventLogDto
    {
        public long id { get; set; }
        public string eventTypeName { get; set; }
        public string domainEventJson { get; set; }
        public long moduleId { get; set; } 
        public DateTime createdOn { get; set; }
    }
}
