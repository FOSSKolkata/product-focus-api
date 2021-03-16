using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class Product : AggregateRoot<long>
    {
        public string Name { get; set; }
        public List<Module> Modules { get; set; }
        public long OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
